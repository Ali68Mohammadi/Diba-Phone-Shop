var dataTable;

$(document).ready(function () {
    loadDataTable();

});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "productId",  //آی دی رکورد حذف یا ویرایش شونده 
                "render": function (data) {
                    return `
                                   <div class="w-75 btn-group" role="group">
                                   <a href="/Admin/Product/Upsert?productId=${data}"
                                   class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                                   <a onClick=Delete('/Admin/Product/Delete?productId=${data}')
                                   class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                </div>
                `
                }

            }
        ]
    }); dataTable.ajax.reload();

}

function Delete(url) {
    Swal.fire({
        title: ' Are you sure ? ',
        text: ' You wont be delete to this !',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '!Yes, delete it'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        swal.fire({

                            title: "!Deleted",
                            text: ".Your product has been deleted",
                            icon: "success"

                        });
                    }
                }
            })

        }

    })
}