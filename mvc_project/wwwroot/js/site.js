// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function postRequest(url, data) 
{
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: data
    })
        .then(responce => {
            if (responce.ok){
                window.location.reload()
            }
        })
        .catch(e => console.error(e))
}

function addToCart(productId)
{
    postRequest('/Cart/AddToCart', JSON.stringify({ productId: productId }))
}

function removeFromCart(productId, quantity = 0)
{
    postRequest('/Cart/RemoveFromCart', JSON.stringify({ productId: productId, quantity: quantity }))
}

function addPromocode()
{
    let id = document.getElementById('promocodeInput').value
    postRequest('/Cart/AddPromoCode', JSON.stringify({id: id}))
}