var dataTable;

$(document).ready(function (){
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblUsers').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id", lockoutEnd : "lockoutEnd"
                },
                "render": function (data) { //render HTML Links
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        // user is currently locked
                        return `     
                                <div class="text-center">
                                    <a onclick=LockUnlock('${data.id}') class="btn btn-danger">Unlock</a>
                                </div>
                            `
                    } else {
                        return `     
                                <div class="text-center">
                                    <a onclick=LockUnlock('${data.id}') class="btn btn-success">Lock</a>
                                </div>
                            `
                    }
                }, "width": "25%"
            },

        ]

    });
}

function LockUnlock(id) {
            $.ajax({
                type: "POST",
                url: '/Admin/User/LockUnlock',
                data: JSON.stringify(id),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
}