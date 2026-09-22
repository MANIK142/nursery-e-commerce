var DataTable;

$(document).ready(function () {
    orderDataTable();
})

function orderDataTable() {
    DataTable =$('#tblData').DataTable({
        ajax: {
            url: '/admin/order/getallorder',
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
  
