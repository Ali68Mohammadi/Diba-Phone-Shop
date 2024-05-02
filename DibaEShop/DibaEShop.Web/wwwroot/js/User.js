var dataTable;

$(document).ready(function () {
    loadDataTable();

});


function loadDataTable() {
    console.log('start')
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAllUser",
            "type": "Get",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",  //آی دی رکورد حذف یا ویرایش شونده 
                "render": function (data) {
                    return `
                                   <div class="w-75 btn-group" role="group">
                                   <a href="/Admin/User/Update?id=${data}"
                                   class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                                   <a onClick=Delete('/Admin/User/Delete?id=${data}')
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
                            text: ".User has been deleted",
                            icon: "success"

                        });
                    }
                }
            })

        }

    })
}