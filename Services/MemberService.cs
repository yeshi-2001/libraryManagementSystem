using System;
using System.Collections.Generic;
using libraryManagementSystem.Models;
using libraryManagementSystem.Repository;
using libraryManagementSystem.Utils;

namespace libraryManagementSystem.Services
{
    /// <summary>
    /// Business logic for Member management. Pages call into this class, never
    /// directly into MemberRepository.
    /// </summary>
    public class MemberService
    {
        private readonly MemberRepository _memberRepository;

        public MemberService()
        {
            _memberRepository = new MemberRepository();
        }

        public List<Member> GetAllMembers()
        {
            return _memberRepository.GetAll();
        }

        public Member GetMemberById(int memberId)
        {
            if (!ValidationHelper.IsPositiveInteger(memberId))
            {
                throw new ArgumentException("Invalid member ID.");
            }

            Member member = _memberRepository.GetById(memberId);
            if (member == null)
            {
                throw new InvalidOperationException("Member not found.");
            }
            return member;
        }

        public PagedResult<Member> SearchMembers(string searchTerm, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = Constants.Pagination.DefaultPageSize;

            return _memberRepository.Search(ValidationHelper.SanitizeInput(searchTerm), pageNumber, pageSize);
        }

        public int AddMember(Member member)
        {
            ValidateMember(member);
            CheckDuplicateNic(member.NIC, excludeMemberId: null);

            member.FullName = ValidationHelper.SanitizeInput(member.FullName.Trim());
            member.NIC = member.NIC.Trim();
            member.Address = ValidationHelper.SanitizeInput(member.Address);
            member.Status = string.IsNullOrEmpty(member.Status) ? Constants.MemberStatus.Active : member.Status;

            return _memberRepository.Insert(member);
        }

        public void UpdateMember(Member member)
        {
            if (!ValidationHelper.IsPositiveInteger(member.MemberId))
            {
                throw new ArgumentException("Invalid member ID.");
            }

            ValidateMember(member);
            CheckDuplicateNic(member.NIC, excludeMemberId: member.MemberId);

            member.FullName = ValidationHelper.SanitizeInput(member.FullName.Trim());
            member.NIC = member.NIC.Trim();
            member.Address = ValidationHelper.SanitizeInput(member.Address);

            _memberRepository.Update(member);
        }

        /// <summary>
        /// Deletes a member. Throws a friendly message if the member has borrow
        /// history and cannot be removed.
        /// </summary>
        public void DeleteMember(int memberId)
        {
            bool deleted = _memberRepository.Delete(memberId);
            if (!deleted)
            {
                throw new InvalidOperationException(
                    "This member cannot be deleted because they have borrow history. " +
                    "Consider setting their status to Inactive instead.");
            }
        }

        /// <summary>
        /// Central rule for whether a member is allowed to borrow right now.
        /// Called by BorrowService before issuing any book — kept here (not
        /// duplicated in BorrowService) since it's fundamentally a Member rule.
        /// </summary>
        public bool IsEligibleToBorrow(Member member)
        {
            return member != null && member.Status == Constants.MemberStatus.Active;
        }

        private void ValidateMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            if (!ValidationHelper.IsNotEmpty(member.FullName))
            {
                throw new ArgumentException("Full name is required.");
            }
            if (!ValidationHelper.IsValidNIC(member.NIC))
            {
                throw new ArgumentException("NIC format is invalid.");
            }
            if (!string.IsNullOrEmpty(member.Email) && !ValidationHelper.IsValidEmail(member.Email))
            {
                throw new ArgumentException("Email format is invalid.");
            }
            if (!string.IsNullOrEmpty(member.Phone) && !ValidationHelper.IsValidPhone(member.Phone))
            {
                throw new ArgumentException("Phone number format is invalid.");
            }
        }

        private void CheckDuplicateNic(string nic, int? excludeMemberId)
        {
            List<Member> allMembers = _memberRepository.GetAll();
            foreach (Member existing in allMembers)
            {
                bool isSameMember = excludeMemberId.HasValue && existing.MemberId == excludeMemberId.Value;
                if (!isSameMember && existing.NIC.Equals(nic?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"A member with NIC '{nic}' already exists.");
                }
            }
        }
    }
}