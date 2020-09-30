$(document).ready(function () { // Ждём загрузки страницы         
    $(".image").click(function () {    // Событие клика на маленькое изображение
      //  var img = $(this);    // Получаем изображение, на которое кликнули
     //   var src = img.attr('src'); // Достаем из этого изображения путь до картинки
        $(".mainblock").append("<div class='popup'>" + //Добавляем в тело документа разметку всплывающего окна
        "<div class='popup_bg'></div><div class='popup_img'><div class='close_cros' style='position: absolute; background-image: url(\"../../../Content/images/close.png\"); width: 32px; height: 32px; right: 0px; cursor: pointer;'></div>" + // Блок, который будет служить фоном затемненным
        "<div>блок</div>" + // Само увеличенное фото
        "</div></div>");
        $(".popup").fadeIn(400); // Медленно выводим изображение
        $(".close_cros").click(function () {    // Событие клика на затемненный фон      
            $(".popup").fadeOut(400);    // Медленно убираем всплывающее окно
            setTimeout(function () {    // Выставляем таймер
                $(".popup").remove(); // Удаляем разметку всплывающего окна
            }, 400);
        });

        $(".popup_bg").click(function () {    // Событие клика на затемненный фон      
            $(".popup").fadeOut(400);    // Медленно убираем всплывающее окно
            setTimeout(function () {    // Выставляем таймер
                $(".popup").remove(); // Удаляем разметку всплывающего окна
            }, 400);
        });
    });

});





