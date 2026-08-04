using System.Collections.Generic;
using System.Data;
using libraryManagementSystem.Models;
using libraryManagementSystem.Utils;

namespace libraryManagementSystem.Repository
{
    /// <summary>
    /// Data access for the Members table. No business logic — validation and
    /// rules (e.g. blocking borrow for inactive members) live in MemberService.
    /// </summary>
    public class MemberRepository
    {
        public List<Member> GetAll()
        {
            DataTable table = Database.ExecuteQuery("sp_Member_GetAll", CommandType.StoredProcedure);
            return MapDataTableToList(table);
        }

        public Member GetById(int memberId)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_Member_GetById",
                CommandType.StoredProcedure,
                Database.Param("@MemberId", memberId));

            return table.Rows.Count > 0 ? MapRowToMember(table.Rows[0]) : null;
        }

        public int Insert(Member member)
        {
            return Database.ExecuteInsertAndGetId(
                "sp_Member_Insert",
                CommandType.StoredProcedure,
                Database.Param("@FullName", member.FullName),
                Database.Param("@NIC", member.NIC),
                Database.Param("@Email", member.Email),
                Database.Param("@Phone", member.Phone),
                Database.Param("@Address", member.Address),
                Database.Param("@Status", member.Status));
        }

        public void Update(Member member)
        {
            Database.ExecuteNonQuery(
                "sp_Member_Update",
                CommandType.StoredProcedure,
                Database.Param("@MemberId", member.MemberId),
                Database.Param("@FullName", member.FullName),
                Database.Param("@NIC", member.NIC),
                Database.Param("@Email", member.Email),
                Database.Param("@Phone", member.Phone),
                Database.Param("@Address", member.Address),
                Database.Param("@Status", member.Status));
        }

        /// <summary>Returns false if blocked because the member has borrow history.</summary>
        public bool Delete(int memberId)
        {
            object result = Database.ExecuteScalar(
                "sp_Member_Delete",
                CommandType.StoredProcedure,
                Database.Param("@MemberId", memberId));

            int code = result == null || result == System.DBNull.Value ? 0 : (int)result;
            return code == 1;
        }

        public PagedResult<Member> Search(string searchTerm, int pageNumber, int pageSize)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_Member_Search",
                CommandType.StoredProcedure,
                Database.Param("@SearchTerm", searchTerm),
                Database.Param("@PageNumber", pageNumber),
                Database.Param("@PageSize", pageSize));

            return new PagedResult<Member>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = MapDataTableToList(table),
                TotalCount = table.Rows.Count > 0 ? (int)table.Rows[0]["TotalCount"] : 0
            };
        }

        private List<Member> MapDataTableToList(DataTable table)
        {
            var members = new List<Member>();
            foreach (DataRow row in table.Rows)
            {
                members.Add(MapRowToMember(row));
            }
            return members;
        }

        private Member MapRowToMember(DataRow row)
        {
            return new Member
            {
                MemberId = (int)row["MemberId"],
                FullName = row["FullName"].ToString(),
                NIC = row["NIC"].ToString(),
                Email = row["Email"] == System.DBNull.Value ? null : row["Email"].ToString(),
                Phone = row["Phone"] == System.DBNull.Value ? null : row["Phone"].ToString(),
                Address = row["Address"] == System.DBNull.Value ? null : row["Address"].ToString(),
                RegisteredDate = (System.DateTime)row["RegisteredDate"],
                Status = row["Status"].ToString()
            };
        }
    }
}
