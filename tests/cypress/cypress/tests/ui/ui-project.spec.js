import { createProject } from '../../support/project';

let projectSlugToDelete = undefined; // Used to delete a project created in another test. Value set in one test, used in another test.
let projectSlugToEdit = undefined; // Used to edit a project created in another test. Value set in one test, used in another test.

describe('UI - Project', () => {
  after(() => {
    cy.deleteAllTestProjects();
  });

  it('can add a project', () => {
    const projectName = `Cypress name ** ${new Date().getTime()}`;
    const projectSlug = `cyp${new Date().getTime()}`;
    const projectDesc = `Cypress desc ${new Date().getTime() + 1}`;
    const projectPriority = 997999;
    const projectHidden = false;

    cy.login();

    createProject(projectName, projectSlug, projectDesc, projectPriority, projectHidden);

    cy.visit('/admin/project');

    cy.get('table')
      .find('tbody tr')
      .contains(projectName) // The project name table cell
      .parent('tr') // The project row
      .as('projectTableRow');

    cy.get('@projectTableRow')
      .find('td')
      .then(($cells) => {
        expect($cells.eq(1), 'second item (priority)').to.contain(projectPriority);
        expect($cells.eq(2), 'third item (name)').to.contain(projectName);
        expect($cells.eq(3), 'fourth item (slug)').to.contain(projectSlug);
        expect($cells.eq(4), 'fifth item (description)').to.contain(projectDesc);
      });

    cy.get('@projectTableRow')
      .find('td')
      .eq(3) // Slug cell (3 is the fourth column in a zero-indexed list)
      .as('projectSlug')
      .then(function () {
        // Test that it is visible from the admin's sidebar menu
        cy.get(`nav a.nav-link[href='admin/project/${this.projectSlug.text()}']`)
          .contains(projectName);

        // Test that it is visible from the normal user's sidebar menu
        cy.visit("/");
        cy.get(`nav a.nav-link[href='project/${this.projectSlug.text()}']`)
          .contains(projectName);

        projectSlugToDelete = this.projectSlug.text();
      });
  });

  // This test should run after "can add a project" above, since it deletes that project
  it('can delete a project', () => {
    cy.login();

    cy.visit('/admin/project');
    cy.waitToAvoidDetachedElements();

    cy.get('table')
      .find('tbody tr td:nth-child(4)') // All Slug table cells
      .contains(projectSlugToDelete) // The correct project slug table cell
      .parent('tr') // The project row
      .as('projectTableRow');

    cy.get('@projectTableRow')
      .find('td')
      .eq(3) // Slug cell (3 is the fourth column in a zero-indexed list)
      .as('projectSlug')
      .then(function () {
        cy.get('@projectTableRow')
          .find('td')
          .last()
          .find('a[aria-label="Delete"]')
          .click();

        cy.get('.modal .btn-danger').contains('Delete').click();

        // Test that it is removed from the admin's sidebar menu
        cy.get(`nav a.nav-link[href='admin/project/${this.projectSlug.text()}']`)
          .should('not.exist');

        // Test that it is removed from the normal user's sidebar menu
        cy.visit("/");
        cy.get(`nav a.nav-link[href='project/${this.projectSlug.text()}']`)
          .should('not.exist');
      });
  });

  it('can add a hidden project', () => {
    const projectName = `Cypress name ** ${new Date().getTime()}`;
    const projectSlug = `cyp${new Date().getTime()}`;
    const projectDesc = `Cypress desc ${new Date().getTime() + 1}`;
    const projectPriority = 997999;
    const projectHidden = true;

    cy.login();

    createProject(projectName, projectSlug, projectDesc, projectPriority, projectHidden);

    cy.visit('/admin/project');

    cy.get('table')
      .find('tbody tr')
      .contains(projectName) // The project name table cell
      .parent('tr') // The project row
      .as('projectTableRow');

    cy.get('@projectTableRow')
      .find('td')
      .then(($cells) => {
        expect($cells.eq(1), 'second item (priority)').to.contain(projectPriority);
        expect($cells.eq(2), 'third item (name)').to.contain(projectName);
        expect($cells.eq(3), 'fourth item (slug)').to.contain(projectSlug);
        expect($cells.eq(4), 'fifth item (description)').to.contain(projectDesc);
      });

    cy.get('@projectTableRow')
      .find('td')
      .eq(3) // Slug cell (3 is the fourth column in a zero-indexed list)
      .as('projectSlug')
      .then(function () {
        cy.get(`nav a.nav-link[href='admin/project/${this.projectSlug.text()}']`)
          .contains(`${projectName} (Hidden)`);

        cy.visit("/");
        cy.get(`nav a.nav-link[href='project/${this.projectSlug.text()}']`)
          .should('not.exist');

        projectSlugToEdit = this.projectSlug.text();
      });
  });

  // This test should run after "can add a hidden project" above, since it edits that project
  it('can edit a project', () => {
    const projectName = `Cypress name ** ${new Date().getTime()}`;
    const projectSlug = `cyp${new Date().getTime()}`;
    const projectDesc = `Cypress desc ${new Date().getTime() + 1}`;
    const projectPriority = 997999;

    cy.login();

    cy.visit('/admin/project');
    cy.waitToAvoidDetachedElements();

    cy.get('table')
      .find('tbody tr td:nth-child(4)') // All Slug table cells
      .contains(projectSlugToEdit) // The correct project slug table cell
      .parent('tr') // The project row
      .as('projectTableRow');

    cy.get('@projectTableRow')
      .find('td')
      .first() // The project ID is the first cell in the row
      .as('projectId')
      .then(function () {
        cy.get('@projectTableRow')
          .find('td')
          .last()
          .find('a[aria-label="Edit"]')
          .click();

        cy.url().should('eq', `${Cypress.config('baseUrl')}/admin/project/${projectSlugToEdit}/edit`);

        cy.waitToAvoidDetachedElements();
        cy.get('input#name').clear().type(projectName).should('have.value', projectName);
        cy.get('input#slug').clear().type(projectSlug).should('have.value', projectSlug);
        cy.get('input#desc').clear().type(projectDesc).should('have.value', projectDesc);
        cy.get('input#sort').clear().type(projectPriority).should('have.value', projectPriority);

        cy.get('input#hidden')
          .uncheck()
          .should('not.be.checked');

        cy.get("button[type='submit']").contains('Update').click();

        cy.url().should('eq', `${Cypress.config('baseUrl')}/admin/project`);

        cy.get('table')
          .find('tbody tr')
          .contains(projectName) // The project name table cell
          .parent('tr') // The project row
          .find('td')
          .then(($cells) => {
            expect($cells.eq(0), 'first item (ID)').to.contain(this.projectId.text());
            expect($cells.eq(1), 'second item (priority)').to.contain(projectPriority);
            expect($cells.eq(2), 'third item (name)').to.contain(projectName);
            expect($cells.eq(3), 'fourth item (slug)').to.contain(projectSlug); // Should be the new value in projectSlug, not the old value in projectSlugToEdit
            expect($cells.eq(4), 'fifth item (description)').to.contain(projectDesc);
          });

        // Test that it is visible from the admin's sidebar menu
        cy.get(`nav a.nav-link[href='admin/project/${projectSlug}']`)
          .contains(projectName);

        // Test that it is visible from the normal user's sidebar menu
        cy.visit("/");
        cy.get(`nav a.nav-link[href='project/${projectSlug}']`)
          .contains(projectName);
      });
  });
});