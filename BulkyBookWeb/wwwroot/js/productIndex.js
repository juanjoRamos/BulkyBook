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
                { data: 'title', width: '20%' },
                {
                    data: 'description', width: '50%', render: (data) => {
                        return data.substring(0, 50) + '...';
                    }
                },
                { data: 'isbn', width: '10%' },
                { data: 'author', width: '10%' },
                { data: 'price', width: '10%' }
            ]
        }

    );
}