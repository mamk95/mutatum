import './commands';

Cypress.on('uncaught:exception', (err, runnable) => {
    // returning false here prevents Cypress from failing the
    // test when an exception is thrown by the webpage or web browser
    return false;
});

// Any cookie defined in 'preserve' will not be cleared before each test runs
Cypress.Cookies.defaults({
    preserve: ['.AspNetCore.Identity.Application'],
});
