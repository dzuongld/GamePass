let dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Platform/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" }, // match with category's Name
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Platform/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-edit"></i>
                            </a>

                            <a onclick=Delete("/Admin/Platform/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </div>
                        `
                },
                "width": "40%"
            }
        ]
    })
}

function Delete(url) {
    // sweet alert
    swal({
        title: "Confirm Delete?",
        text: "This action cannot be reversed!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    // variables are defined in controller action
                    if (data.success) {
                        // toastr notification
                        toastr.success(data.message)
                        dataTable.ajax.reload()
                    } else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}