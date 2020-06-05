var dataTable;

$(document).ready(function (){
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblProducts').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "listPrice", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id", // RENDER ID FROM MODEL
                "render": function (data) { //render HTML Links
                    return `     
                                <div class="text-center">
                                    <a href="/Admin/Product/Upsert/${data}" class="btn btn-outline-primary">Edit</a>
                                    <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-outline-primary">Delete</a>
                                </div>
                            `
                }, "width": "25%"
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