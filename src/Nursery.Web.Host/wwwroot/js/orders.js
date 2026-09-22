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
            { data: 'id', width: '25%' },
            { data: 'total', width: '10%' },
            { data: 'orderItems', width: '30%' },
            { data: 'status', width: '10%' },
            { data: 'paymentStatus', width: '10%' },
            {
                data: 'id', width: 'auto', render: function (data) {
                    return `<div class="d-flex gap-2 justify-content-end">
                                         <a href="/admin/order/update?id=${data}" class="btn btn-sm btn-outline-success">
                                              <i class="bi bi-pencil-square"></i> Update
                                         </a>
                         </div > `;
                }
            }
        ]
    });
}
  
