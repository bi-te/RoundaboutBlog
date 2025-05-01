document.addEventListener('DOMContentLoaded', function() {
    const containers = document.querySelectorAll('.post-container');

    containers.forEach(container => {
        const content = container.querySelector('.post-content');
        const button = container.querySelector('.toggle-btn');

        if (content.scrollHeight <= content.clientHeight) {
            button.style.display = 'none';
        }
        else
        {
            button.addEventListener('click', function() {
                if (container.classList.toggle('expanded'))
                {
                    button.innerHTML = "Show less";
                    content.style.maxHeight = content.scrollHeight + 'px';
                }
                else
                {
                    button.innerHTML = "Read more";
                    content.style.maxHeight = 'calc(1.5em * 4)'
                }
            });
        }
    });
});