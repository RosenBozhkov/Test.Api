function attachLinkToLogo() {
    let checkExists = setInterval(() => {
        let logo = document.querySelector('#swagger-ui > section > div.topbar > div > div > a');
        if (logo != null) {
            clearInterval(checkExists);
            logo.addEventListener('click', () => window.location.href = "https://itsoft.bg/");
        }
    }, 100);
}

window.addEventListener('load',  attachLinkToLogo);
