window.addEventListener('click', function (event) {
    // Находим счетчик товаров в корзине
    const counterCart = document.querySelector('.counter__cart');

    // Обработка нажатия на кнопку "плюс"
    if (event.target.dataset.action === 'plus') {
        // Получаем текущее количество товаров в корзине
        let currentCount = parseInt(counterCart.innerText);
        // Увеличиваем количество товаров на 1, если не достигнуто максимальное значение
        if (currentCount < 20) {
            counterCart.innerText = currentCount + 1;
        }
    }

    // Обработка нажатия на кнопку "минус"
    if (event.target.dataset.action === 'minus') {
        // Получаем текущее количество товаров в корзине
        let currentCount = parseInt(counterCart.innerText);
        // Уменьшаем количество товаров на 1, если оно больше 0
        if (currentCount > 0) {
            counterCart.innerText = currentCount - 1;
        }
    }
});

// Получаем все элементы с классом "items__control" в каждой карточке продукта
const itemControls = document.querySelectorAll('.items__control');

// Перебираем все найденные элементы
itemControls.forEach(function(itemControl) {
    // Добавляем обработчик события для каждого элемента
    itemControl.addEventListener('click', function(event) {
        // Находим родительский элемент карточки продукта
        const cardBody = event.target.closest('.card-body');
        // Находим счетчик внутри данной карточки продукта
        const counter = cardBody.querySelector('.items__current');
        // Получаем текущее значение счетчика
        let currentValue = parseInt(counter.innerText);

        // Если была нажата кнопка "плюс"
        if (event.target.dataset.action === 'plus') {
            // Увеличиваем значение счетчика на 1, если не достигнут максимум
            if (currentValue < 20) {
                counter.innerText = currentValue + 1;
            }
        }
        // Если была нажата кнопка "минус"
        else if (event.target.dataset.action === 'minus') {
            // Уменьшаем значение счетчика на 1, если не достигнут минимум
            if (currentValue > 0) {
                counter.innerText = currentValue - 1;
            }
        }
    });
});

var addToCartButtons = document.querySelectorAll('[data-cart]');

addToCartButtons.forEach(function(button) {
    button.addEventListener('click', function() {
        var cardBody = button.closest('.card-body');
        var counter = cardBody.querySelector('[data-counter]');
        var itemCount = parseInt(counter.innerText);
        var itemName = cardBody.closest('.card').dataset.name; // Получаем имя товара

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Buy/AddToCart', true); // Используйте правильный маршрут к вашему методу контроллера
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {
                console.log('Товар успешно добавлен в корзину');
            } else {
                console.error('Произошла ошибка при добавлении товара в корзину');
            }
        };

        xhr.send(JSON.stringify({ name: itemName, quantity: itemCount })); // Отправляем на сервер имя и количество товара
    });
});



let askedBox = document.querySelectorAll('.asked__box')
let btn = document.querySelectorAll('#btn')
let span = document.querySelectorAll('.span')

askedBox.forEach(el => {
    el.addEventListener('click', () => {
        let span = el.childNodes[5]
        let btn = el.childNodes[3]
        let askedBox = el
        if(span.style.maxHeight == `0px`){
            span.style.maxHeight = span.scrollHeight + `px`
            btn.style.transform = 'rotate(180deg)'
            askedBox.style.background = '#fff'
        }else{
            span.style.maxHeight = 0
            btn.style.transform = 'rotate(0deg)'
            askedBox.style.background = '#FCFAFF'
        }
    })
})

// CART

// const cartWrapper =  document.querySelector('.section__btn');
//
// // Отслеживаем клик на странице
// window.addEventListener('click', function (event) {
//     // Проверяем что клик был совершен по кнопке "Добавить в корзину"
//     if (event.target.hasAttribute('data-cart')) {
//
//         // Находим карточку с товаром, внутри котрой был совершен клик
//         const card = event.target.closest('.card');
//
//         // Собираем данные с этого товара и записываем их в единый объект productInfo
//         const productInfo = {
//             id: card.dataset.id,
//             imgSrc: card.querySelector('.product-img').getAttribute('src'),
//             title: card.querySelector('.item-title').innerText,
//             itemsInBox: card.querySelector('[data-items-in-box]').innerText,
//             weight: card.querySelector('.price__weight').innerText,
//             price: card.querySelector('.price__currency').innerText,
//             counter: card.querySelector('[data-counter]').innerText,
//         };
//
//         // Проверять если ли уже такой товар в корзине
//         const itemInCart = cartWrapper.querySelector(`[data-id="${productInfo.id}"]`);
//
//         // Если товар есть в корзине
//         if (itemInCart) {
//             const counterElement = itemInCart.querySelector('[data-counter]');
//             counterElement.innerText = parseInt(counterElement.innerText) + parseInt(productInfo.counter);
//         } else {
//             // Если товара нет в корзине
//
//             // Собранные данные подставим в шаблон для товара в корзине
//             const cartItemHTML = `<div class="cart-item" data-id="${productInfo.id}">
// 								<div class="cart-item__top">
// 									<div class="cart-item__img">
// 										<img src="${productInfo.imgSrc}" alt="${productInfo.title}">
// 									</div>
// 									<div class="cart-item__desc">
// 										<div class="cart-item__title">${productInfo.title}</div>
// 										<div class="cart-item__weight">${productInfo.itemsInBox} / ${productInfo.weight}</div>
//
// 										<!-- cart-item__details -->
// 										<div class="cart-item__details">
//
// 											<div class="items items--small counter-wrapper">
// 												<div class="items__control" data-action="minus">-</div>
// 												<div class="items__current" data-counter="">${productInfo.counter}</div>
// 												<div class="items__control" data-action="plus">+</div>
// 											</div>
//
// 											<div class="price">
// 												<div class="price__currency">${productInfo.price}</div>
// 											</div>
//
// 										</div>
// 										<!-- // cart-item__details -->
//
// 									</div>
// 								</div>
// 							</div>`;
//
//             // Отобразим товар в корзине
//             cartWrapper.insertAdjacentHTML('beforeend', cartItemHTML);
//         }
//
//         // Сбрасываем счетчик добавленного товара на "1"
//         card.querySelector('[data-counter]').innerText = '1';
//
//         // Отображение статуса корзины Пустая / Полная
//         toggleCartStatus();
//
//         // Пересчет общей стоимости товаров в корзине
//         calcCartPriceAndDelivery();
//
//     }
// });
