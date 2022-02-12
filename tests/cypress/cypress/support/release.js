export function createRelease(projectSlug, title, hidden, shortDesc, longDesc, versionMajor, versionMinor, versionPatch, releaseYear, releaseMonth, releaseDay) {
    cy.visit(`/admin/project/${projectSlug}`);

    cy.waitToAvoidDetachedElements();
    cy.get('a').contains('Create new release').click();
    cy.url().should('eq', `${Cypress.config('baseUrl')}/admin/project/${projectSlug}/release/new`);

    cy.waitToAvoidDetachedElements();
    cy.get('input#title').type(title).should('have.value', title);

    if (hidden === true) {
        cy.get('input#hidden')
            .check()
            .should('be.checked');
    } else if (hidden === false) {
        cy.get('input#hidden').should('not.be.checked');
    } else {
        throw new Error("projectHidden must be a boolean");
    }

    cy.get('textarea#shortdesc').type(shortDesc).should('have.value', shortDesc);

    cy.get('textarea#longdesc')
        .next()
        .find('.CodeMirror-scroll')
        .type(longDesc, { delay: 5 });
    cy.get('textarea#longdesc')
        .should('have.value', longDesc);

    if (versionMajor != null) cy.get('input#version-major').clear().type(versionMajor).should('have.value', versionMajor);
    if (versionMinor != null) cy.get('input#version-minor').clear().type(versionMinor).should('have.value', versionMinor);
    if (versionPatch != null) cy.get('input#version-patch').clear().type(versionPatch).should('have.value', versionPatch);

    cy.get('input#release-date-year').clear().type(releaseYear).should('have.value', releaseYear);
    cy.get('input#release-date-month').clear().type(releaseMonth).should('have.value', releaseMonth);
    cy.get('input#release-date-day').clear().type(releaseDay).should('have.value', releaseDay);

    cy.get("button[type='submit']").contains('Create').click();

    cy.url().should('eq', `${Cypress.config('baseUrl')}/admin/project/${projectSlug}`);
}