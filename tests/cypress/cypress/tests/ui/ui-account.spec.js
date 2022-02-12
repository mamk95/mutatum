function registerAccount(email, password, verifyRegistrationConfirmation = true) {
    cy.visit('https://localhost:7028/Identity/Account/Register');

    cy.get('#Input_Email').type(email, { delay: 0 });
    cy.get('#Input_Password').type(password, { log: false, delay: 0 });
    cy.get('#Input_ConfirmPassword').type(`${password}{enter}`, { log: false, delay: 0 }); // 'Enter' to submit form

    if (verifyRegistrationConfirmation) {
        cy.contains('Register confirmation');
        cy.url().should('eq', `${Cypress.config('baseUrl')}/Identity/Account/RegisterConfirmation?email=${email}&returnUrl=%2F`);
    }
}

describe('UI - Account', () => {
    beforeEach(() => {
        // Remove auth cookie in case Cypress has logged us in in another test
        cy.clearCookie('.AspNetCore.Identity.Application');
    });

    after(() => {
        // Remove auth cookie so other Cypress tests don't use our test accounts
        cy.clearCookie('.AspNetCore.Identity.Application');
    });

    it('cannot login with fake account', () => {
        cy.login('fake-account@example.com', 'fake-password', true, false);

        cy.contains('Invalid login attempt');

        // Try to visit page that requires you to be logged in
        cy.visit('/admin/project');
        cy.url().should('eq', `${Cypress.config('baseUrl')}/Identity/Account/Login?returnUrl=/admin/project`);
    });

    it('can login with admin account', () => {
        cy.login(null, null, true, true);

        // Try to visit page that requires you to be logged in
        cy.visit('/admin/project');
        cy.url().should('not.eq', `${Cypress.config('baseUrl')}/Identity/Account/Login?returnUrl=/admin/project`);
    });

    it('can register new account', () => {
        registerAccount('new-account@example.com', 'MyPassword123!');
    });

    // Uses account email and password from the test 'can register new account' above
    it('cannot login with new account', () => {
        cy.login('new-account@example.com', 'MyPassword123!', true, false);

        cy.contains('User not allowed to login. If this is a new account, make sure an existing user/admin has approved your account.');

        // Try to visit page that requires you to be logged in
        cy.visit('/admin/project');
        cy.url().should('eq', `${Cypress.config('baseUrl')}/Identity/Account/Login?returnUrl=/admin/project`);
    });

    // Uses account email and password from the test 'can register new account' above
    it('admin can activate new account', () => {
        cy.login(null, null, true, true);

        cy.visit('/admin/user');
        cy.waitToAvoidDetachedElements();

        cy.get('table')
            .contains('new-account@example.com')
            .parent('tr')
            .find('td')
            .then(($cells) => {
                expect($cells.eq(1), 'second item (username)').to.contain('new-account@example.com');
                expect($cells.eq(2), 'third item (activated)').to.contain('False');
            });

        cy.get('table')
            .contains('new-account@example.com')
            .parent('tr')
            .find('a')
            .contains('Click to activate')
            .click();

        cy.wait(1000); // Wait for the account to be activated. Can be slow depending on the database

        cy.get('table')
            .contains('new-account@example.com')
            .parent('tr')
            .find('td')
            .then(($cells) => {
                expect($cells.eq(1), 'second item (username)').to.contain('new-account@example.com');
                expect($cells.eq(3), 'fourth item (activated)').to.contain('True');
            });

        // Login with new account
        cy.clearCookie('.AspNetCore.Identity.Application');
        cy.login('new-account@example.com', 'MyPassword123!', true, true);

        // Try to visit page that requires you to be logged in
        cy.visit('/admin/project');
        cy.url().should('not.eq', `${Cypress.config('baseUrl')}/Identity/Account/Login?returnUrl=/admin/project`);
    });

    it('locks account on too many failed login attempts', () => {
        cy.login('new-account@example.com', 'WrongPassword', true, false);
        cy.login('new-account@example.com', 'WrongPassword', true, false);
        cy.login('new-account@example.com', 'WrongPassword', true, false);
        cy.login('new-account@example.com', 'WrongPassword', true, false);

        cy.url().should('eq', `${Cypress.config('baseUrl')}/Identity/Account/Lockout`);

        // Clear cookies and try one last time
        cy.clearCookie('.AspNetCore.Identity.Application');
        cy.login('new-account@example.com', 'WrongPassword', true, false);
        cy.url().should('eq', `${Cypress.config('baseUrl')}/Identity/Account/Lockout`);
    });

    // Uses account email and password from the test 'can register new account' above
    it('admin can deleted activated account', () => {
        cy.login(null, null, true, true);

        cy.visit('/admin/user');
        cy.waitToAvoidDetachedElements();

        cy.get('table')
            .contains('new-account@example.com')
            .parent('tr')
            .find('td')
            .then(($cells) => {
                expect($cells.eq(1), 'second item (username)').to.contain('new-account@example.com');
                expect($cells.eq(3), 'fourth item (activated)').to.contain('True');
            });

        cy.get('table')
            .contains('new-account@example.com')
            .parent('tr')
            .find('a[aria-label="Delete"]')
            .click();

        cy.get('.modal .btn-danger').contains('Delete').click();

        cy.wait(1000); // Wait for the account to be deleted. Can be slow depending on the database

        cy.contains('new-account@example.com').should('not.exist');

        // Login with deleted account
        cy.clearCookie('.AspNetCore.Identity.Application');
        cy.login('new-account@example.com', 'MyPassword123!', true, false);

        // Try to visit page that requires you to be logged in
        cy.visit('/admin/project');
        cy.url().should('eq', `${Cypress.config('baseUrl')}/Identity/Account/Login?returnUrl=/admin/project`);
    });

    it('admin can delete new unactivated account', () => {
        registerAccount('new-account-2@example.com', 'MyPassword123!');

        // Login to admin account to delete the new unactivated account
        cy.clearCookie('.AspNetCore.Identity.Application');
        cy.login(null, null, true, true);
        cy.visit('/admin/user');
        cy.waitToAvoidDetachedElements();

        cy.get('table')
            .contains('new-account-2@example.com')
            .parent('tr')
            .find('a[aria-label="Delete"]')
            .click();

        cy.get('.modal .btn-danger').contains('Delete').click();

        cy.wait(1000); // Wait for the account to be deleted. Can be slow depending on the database

        // Login with deleted account
        cy.clearCookie('.AspNetCore.Identity.Application');
        cy.login('new-account-2@example.com', 'MyPassword123!', true, false);

        cy.contains('Invalid login attempt');

        // Try to visit page that requires you to be logged in
        cy.visit('/admin/project');
        cy.url().should('eq', `${Cypress.config('baseUrl')}/Identity/Account/Login?returnUrl=/admin/project`);
    });
});