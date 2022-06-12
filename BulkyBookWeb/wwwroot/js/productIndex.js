var dataTableProduct;

window.onload = () => {
    loadDataTableProducts();
};

function loadDataTableProducts()
{
    dataTableProduct = $('#datatableProducts').DataTable(
        {
            responsive: true,
            searching: false,
            scrollY: '500px',
            scrollCollapse: true,
            ordering: false,
            paging: false,
            ajax: {
                url: '/Admin/Product/GetAll'
            },
            columns: [
                { data: 'title', width: '12.5%' },
                {
                    data: 'description', width: '12.5%', render: (data) => {
                        return data.substring(0, 50) + '...';
                    }
                },
                { data: 'isbn', width: '12.5%' },
                { data: 'author', width: '12.5%' },
                { data: 'price', width: '12.5%' },
                { data: 'category.name', width: '12.5%' },
                { data: 'coverType.name', width: '12.5%' },
                {
                    data: 'id',
                    render: (data) =>
                    {
                        return `<div class="w-75 btn-group" role="group">
                                <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2">
                                    <i class="bi bi-pencil-square"></i> Edit
                                </a>
                                <a href="/Admin/Product/" class="btn btn-danger mx-2">
                                    <i class="bi bi-trash-fill"></i> Delete
                                </a>
                            </div>`;
                    },
                    width: '12.5%'
                }
            ],
            columnDefs: [{
                "defaultContent": "",
                "targets": "_all"
            }]
        }

    );
}