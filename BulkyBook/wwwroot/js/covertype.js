var dataTable;

$(document).ready(function (){
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblCoverType').DataTable({
        "ajax": {
            "url": "/Admin/CoverType/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "id", // RENDER ID FROM MODEL
                "render": function (data) { //render HTML Links
                    return `     
                                <div class="text-center">
                                    <a href="/Admin/CoverType/Upsert/${data}" class="btn btn-outline-primary">Edit</a>
                                    <a onclick=Delete("/Admin/CoverType/Delete/${data}") class="btn btn-outline-primary">Delete</a>
                                </div>
                            `
                }, "width": "40%"
            },

        ]

    });
}

function Delete(url) {
    swal({
        title: "Are your sure you want to delete the record?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((userResponse) => {
        if (userResponse) {
            $.ajax({
                type: "DELETE",
                url: url,
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
    });
}