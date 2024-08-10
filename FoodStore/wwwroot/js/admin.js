// Define updateStatus function globally
function updateStatus(orderId, newStatus) {
    console.log("Order ID: ", orderId); // Debugging
    console.log("New Status: ", newStatus); // Debugging
    $.ajax({
        url: `/Admin/Order/UpdateStatus`,
        type: 'POST',
        data: {
            orderId: orderId,
            newStatus: newStatus
        },
        success: function (response) {
            if (response.success) {
                alert('Status updated successfully!');
                // Update the status text directly instead of reloading the page
                $('#statusDisplay').text(newStatus === "1" ? "Delivered" : newStatus === "2" ? "Cancelled" : "Pending");
            } else {
                alert('Failed to update status');
            }
        },
        error: function () {
            alert('Error updating status');
        }
    });
}

function refreshStatus(orderId) {
    $.ajax({
        url: `/Admin/Order/GetCurrentStatus`,
        type: 'GET',
        data: { orderId: orderId },
        success: function (response) {
            $('#statusText').text(response.status);  // Assuming response.status holds the new status text
        },
        error: function () {
            alert('Error fetching status');
        }
    });
}



$(document).ready(function () {
    $('#productsTable').DataTable();
});
