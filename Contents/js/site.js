// ============================================================
// site.js - shared client-side behavior across the whole app
// ============================================================

/**
 * Shows a Bootstrap 5 toast notification.
 * @param {string} message - text to display
 * @param {string} type - 'success' | 'danger' | 'warning' | 'info'
 */
function showToast(message, type) {
    type = type || 'success';

    var container = document.getElementById('toastContainer');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toastContainer';
        container.className = 'toast-container position-fixed top-0 end-0 p-3';
        container.style.zIndex = '1080';
        document.body.appendChild(container);
    }

    var toastEl = document.createElement('div');
    toastEl.className = 'toast align-items-center text-white bg-' + type + ' border-0';
    toastEl.setAttribute('role', 'alert');
    toastEl.innerHTML =
        '<div class="d-flex">' +
        '  <div class="toast-body">' + message + '</div>' +
        '  <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>' +
        '</div>';

    container.appendChild(toastEl);
    var toast = new bootstrap.Toast(toastEl, { delay: 4000 });
    toast.show();

    toastEl.addEventListener('hidden.bs.toast', function () {
        toastEl.remove();
    });
}

function confirmDelete(itemName) {
    return confirm('Are you sure you want to delete ' + (itemName || 'this item') + '? This action cannot be undone.');
}

/**
 * Toggles the sidebar on small screens (hamburger button in the topbar).
 */
function toggleSidebar() {
    var sidebar = document.querySelector('.lms-sidebar');
    if (sidebar) {
        sidebar.classList.toggle('show');
    }
}

// Auto-show any toast the server queued via a hidden field (see Site.Master pattern)
document.addEventListener('DOMContentLoaded', function () {
    var queuedToast = document.getElementById('hfToastMessage');
    if (queuedToast && queuedToast.value) {
        var type = document.getElementById('hfToastType');
        showToast(queuedToast.value, type ? type.value : 'success');
    }
});