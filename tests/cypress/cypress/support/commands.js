/**
 * Avoid anoying Cypress bug which does not re-query a selector if the element is detached from the DOM.
 * The element will sometimes be re-rendered right after page load, failing the test.
 * There are a lot of really complicated ways to avoid this. The easiest way is to just wait a bit.
 * Cypress bug: https://github.com/cypress-io/cypress/issues/7306
 */
Cypress.Commands.add('waitToAvoidDetachedElements', () => {
    // In my tests, 900ms seems to be the lowest you can reliably go
    cy.wait(900);
});

Cypress.Commands.add('login', () => {

    cy.getCookie('.AspNetCore.Identity.Application').then((cookie) => {
        if (cookie == null) {
            cy.visit('/Identity/Account/Login?returnUrl=/admin/project');

            if (Cypress.env('ADMIN_EMAIL') == null) {
                throw new Error("Admin email is missing. Please set it in the environment variables!");
            }

            if (Cypress.env('ADMIN_PASSWORD') == null) {
                throw new Error("Admin password is missing. Please set it in the environment variables!");
            }

            cy.get('#Input_Email').type(Cypress.env('ADMIN_EMAIL'));
            cy.get('#Input_Password').type(`${Cypress.env('ADMIN_PASSWORD')}{enter}`, { log: false }); // "Enter" to submit form



            cy.url().should('eq', `${Cypress.config('baseUrl')}/admin/project`);
        } else {
            cy.log('Already logged in');
        }
    });
});

// Will delete all projects that has names that start with "Cypress name ** "
Cypress.Commands.add('deleteAllTestProjects', () => {
    const namePattern = "Cypress name ** ";

    cy.login();

    cy.visit('/admin/project');

    // The two next lines (get body and body.find) is to avoid Cypress failing the test if no projects are found
    cy.get('body', { log: false }).then((body) => {
        const cypressProjectCount = body.find(`table tbody tr:contains(${namePattern})`).length;

        cy.log(`Deleting ${cypressProjectCount} Cypress test project(s)`);

        for (let i = 0; i < cypressProjectCount; i++) {
            cy.waitToAvoidDetachedElements();

            cy.get('table tbody tr') // All rows
                .contains(namePattern) // All Cypress project name table cells
                .parent('tr') // The project rows
                .first() // First Cypress test project in list
                .find('td')
                .last()
                .find('a[aria-label="Delete"]')
                .click();

            cy.get('.modal .btn-danger').contains('Delete').click();
        }
    });
});