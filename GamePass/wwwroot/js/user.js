let dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" }, // match with category's Name
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id",
                    lockoutEnd: "lockoutEnd" // field in table
                },
                "render": function (data) {
                    var today = new Date().getTime()
                    var lockout = new Date(data.lockoutEnd).getTime()

                    //locked 
                    if (lockout > today) {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock("${data.id}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="fas fa-lock-open"></i> Unlock
                            </a>
                        </div>
                        `
                    }
                    else {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock("${data.id}") class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="fas fa-lock"></i> Lock
                            </a>
                        </div>
                        `
                    }

                },
                "width": "25%"
            }
        ]
    })
}

function LockUnlock(id) {

    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
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