var DataTable;

$(document).ready(function () {
    plantDataTable();
})

function plantDataTable() {
    DataTable =$('#tblData').DataTable({
        ajax: {
            url: '/admin/plant/getall',
            type: 'GET',
            dataSrc: 'data', // remove or change to '' if your controller returns a raw array
            error: function (xhr, error, code) {
                console.error("Status:", xhr.status);
                console.error("Response:", xhr.responseText);
            }
        },
        columns: [
            { data: 'name', width: '15%' },
            {
                data: 'categories',
                width: '30%',
                render: function (data) {
                    return Array.isArray(data) ? data.join(', ') : data;
                }
            },
            { data: 'displayPrice', width: '10%' },
            {
                data: 'id', width: '30%', render: function (data) {
                    return `<div class="d-flex gap-2 justify-content-end">
                                         <a href="/admin/plant/update?id=${data}" class="btn btn-sm btn-outline-success">
                                              <i class="bi bi-pencil-square"></i> Edit
                                         </a>
                                          <a href="/admin/plant/CreateCareInstruction/${data}" class="btn btn-sm btn-outline-secondary">
                                              <i class="bi bi-plus"></i> Add Care
                                         </a>
                                          <a onclick="Delete('/admin/plant/delete/${data}')" class="btn btn-sm btn-outline-danger">
                                              <i class="bi bi-trash"></i> Delete
                                         </a>
                         </div > `;
                }
            }
        ]
    });
}
  


function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    DataTable.ajax.reload(null, false);
                    Swal.fire({
                        title: "Deleted!",
                        text: "Your file has been deleted.",
                        icon: "success"
                    });
                },
                error: function (xhr) {
                    Swal.fire({
                        title: "Error!",
                        text: xhr.responseText || "Failed to delete plant.",
                        icon: "error"
                    });
                }
            })
        }
    });
}