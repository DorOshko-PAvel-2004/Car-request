var items = document.querySelectorAll("li");
for (var i = 0; i < items.length; i++) {
    items[i].addEventListener("click", function () {
        this.classList.toggle("active");
    });
}
