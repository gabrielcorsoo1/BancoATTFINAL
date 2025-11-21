document.addEventListener("DOMContentLoaded", function () {
    var autoCloseAlert = document.getElementById("autoCloseAlert");

    if (autoCloseAlert) {
        var alertTime = 3000; 
        var timeLeft = alertTime;
        var intervalTime = 100; 

        var interval = setInterval(function () {
            if (!autoCloseAlert.matches(':hover')) {
                timeLeft -= intervalTime;
            }

            if (timeLeft <= 0) {
                clearInterval(interval);
                var bsAlert = new bootstrap.Alert(autoCloseAlert);
                bsAlert.close();
            }
        }, intervalTime);
    }

    // Classe que ativa animações de entrada (evita execução antes do DOM pronto)
    document.body.classList.add("page-loaded");

    // Small enhancement: focus primeiro input em formulários de Create para apresentação
    try {
        var createForms = document.querySelectorAll('form[asp-action="Create"]');
        if (createForms.length) {
            var firstInput = createForms[0].querySelector('input.form-control, textarea, select');
            if (firstInput) firstInput.focus();
        }
    } catch (e) {
        // fail silently (compatibilidade)
    }
});

/* Hero background para a página inicial (usar wwwroot/images/aviao.png) */
.hero {
    background-image: url('/images/aviao.png');
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    color: #ffffff;
    padding: 4.5rem 0;
    position: relative;
    border-radius: 8px;
    overflow: hidden;
}

/* overlay para garantir legibilidade do texto */
.hero::before {
    content: "";
    position: absolute;
    inset: 0;
    background: rgba(0,0,0,0.35);
    z-index: 0;
}

/* garante que o conteúdo fique acima do overlay */
.hero .container-fluid,
.hero h1,
.hero p {
    position: relative;
    z-index: 1;
}

/* ajuste responsivo: texto menor em telas pequenas */
@media (max-width: 767.98px) {
    .hero {
        padding: 3rem 0;
    }
    .hero .display-5 {
        font-size: 1.8rem;
    }
    .hero .fs-4 {
        font-size: 1rem;
    }
}