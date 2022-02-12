export function createProject(projectName, projectSlug, projectDesc, projectPriority, projectHidden) {
    cy.visit('/admin/project');

    cy.waitToAvoidDetachedElements();
    cy.get('a').contains('Create new project').click();
    cy.url().should('eq', `${Cypress.config('baseUrl')}/admin/project/new`);

    cy.waitToAvoidDetachedElements();
    cy.get('input#name').type(projectName).should('have.value', projectName);
    cy.get('input#slug').type(projectSlug).should('have.value', projectSlug);
    cy.get('input#desc').type(projectDesc).should('have.value', projectDesc);
    cy.get('input#sort').clear().type(projectPriority).should('have.value', projectPriority);

    if (projectHidden === true) {
        cy.get('input#hidden')
            .check()
            .should('be.checked');
    } else if (projectHidden === false) {
        cy.get('input#hidden').should('not.be.checked');
    } else {
        throw new Error("projectHidden must be a boolean");
    }

    cy.get("button[type='submit']").contains('Create').click();

    cy.url().should('eq', `${Cypress.config('baseUrl')}/admin/project`);
}