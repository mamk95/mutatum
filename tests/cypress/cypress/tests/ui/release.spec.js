import { createProject } from '../../support/project';
import { createRelease } from '../../support/release';

const projectName = `Cypress name ** release ${new Date().getTime()}`;
const projectDesc = `Cypress desc ${new Date().getTime() + 1}`;
const projectPriority = 997999;
const projectHidden = false;
let projectId = undefined;

describe('Release', () => {
    before(() => {
        cy.login();
        createProject(projectName, projectDesc, projectPriority, projectHidden);

        cy.visit('/admin/project');

        // Get the project ID
        cy.get('table')
            .find('tbody tr')
            .contains(projectName) // The project name table cell
            .parent('tr') // The project row
            .find('td')
            .first()
            .as('projectId')
            .then(function () {
                projectId = +this.projectId.text();
            });
    });

    after(() => {
        cy.deleteAllTestProjects();
    });

    it('can add a release with version', () => {
        const releaseTitle = `Cypress title ** ${new Date().getTime()}`;
        const releaseHidden = false;
        const releaseShortDesc = `Cypress short desc ${new Date().getTime() + 1}`;
        const releaseLongDesc = `Cypress long desc ${new Date().getTime() + 2}`;
        const releaseMajor = 3;
        const releaseMinor = 5;
        const releasePatch = 6;
        const releaseYear = 2021;
        const releaseMonth = 12;
        const releaseDay = 21;

        cy.login();

        createRelease(projectId, releaseTitle, releaseHidden, releaseShortDesc, releaseLongDesc, releaseMajor, releaseMinor, releasePatch, releaseYear, releaseMonth, releaseDay);

        cy.get('.release-item')
            .contains(releaseTitle)
            .parents('.release-item')
            .as('release');

        cy.get('@release')
            .find('.release-title')
            .contains(`${releaseMajor}.${releaseMinor}.${releasePatch} - ${releaseTitle}`);

        cy.get('@release')
            .find('.release-date')
            .contains(`${releaseYear}/${releaseMonth}/${releaseDay}`);

        cy.get('@release')
            .find('.release-longdesc')
            .invoke('text')
            .invoke('trim')
            .should('eq', releaseLongDesc);

        // Test that it is visible from the normal user's release view
        cy.visit(`/project/${projectId}`);
        cy.get('.release-item')
            .contains(releaseTitle);
    });

    it('can add a release without version', () => {
        const releaseTitle = `Cypress title ** ${new Date().getTime()}`;
        const releaseHidden = false;
        const releaseShortDesc = `Cypress short desc ${new Date().getTime() + 1}`;
        const releaseLongDesc = `Cypress long desc ${new Date().getTime() + 2}`;
        const releaseYear = 2021;
        const releaseMonth = 12;
        const releaseDay = 21;

        cy.login();

        createRelease(projectId, releaseTitle, releaseHidden, releaseShortDesc, releaseLongDesc, null, null, null, releaseYear, releaseMonth, releaseDay);

        cy.get('.release-item')
            .contains(releaseTitle)
            .parents('.release-item')
            .as('release');

        cy.get('@release')
            .find('.release-title')
            .contains(releaseTitle);

        cy.get('@release')
            .find('.release-date')
            .contains(`${releaseYear}/${releaseMonth}/${releaseDay}`);

        cy.get('@release')
            .find('.release-longdesc')
            .invoke('text')
            .invoke('trim')
            .should('eq', releaseLongDesc);
    });

    it('can add a hidden release', () => {
        const releaseTitle = `Cypress title ** ${new Date().getTime()}`;
        const releaseHidden = true;
        const releaseShortDesc = `Cypress short desc ${new Date().getTime() + 1}`;
        const releaseLongDesc = `Cypress long desc ${new Date().getTime() + 2}`;
        const releaseYear = 2021;
        const releaseMonth = 12;
        const releaseDay = 21;

        cy.login();

        createRelease(projectId, releaseTitle, releaseHidden, releaseShortDesc, releaseLongDesc, null, null, null, releaseYear, releaseMonth, releaseDay);

        cy.get('.release-item')
            .contains(`${releaseTitle} (Hidden)`)
            .parents('.release-item')
            .as('release');

        cy.get('@release')
            .find('.release-title')
            .contains(`${releaseTitle} (Hidden)`);

        cy.get('@release')
            .find('.release-date')
            .contains(`${releaseYear}/${releaseMonth}/${releaseDay}`);

        cy.get('@release')
            .find('.release-longdesc')
            .invoke('text')
            .invoke('trim')
            .should('eq', releaseLongDesc);

        // Test that it is not visible from the normal user's release view
        cy.visit(`/project/${projectId}`);
        cy.get('.release-item')
            .should('not.contain', releaseTitle);
    });

    it('can delete a release', () => {
        const releaseTitle = `Cypress title ** delete me ${new Date().getTime()}`;
        const releaseHidden = false;
        const releaseShortDesc = `Cypress short desc ${new Date().getTime() + 1}`;
        const releaseLongDesc = `Cypress long desc ${new Date().getTime() + 2}`;
        const releaseYear = 2021;
        const releaseMonth = 12;
        const releaseDay = 21;

        cy.login();

        createRelease(projectId, releaseTitle, releaseHidden, releaseShortDesc, releaseLongDesc, null, null, null, releaseYear, releaseMonth, releaseDay);
        cy.waitToAvoidDetachedElements();

        cy.get('.release-item')
            .contains(releaseTitle)
            .parents('.release-item')
            .as('release');

        cy.get('@release')
            .find('a[aria-label="Delete"]')
            .click();

        cy.get('.modal .btn-danger').filter(':visible').contains('Delete').click();

        cy.get('.release-item')
            .contains(releaseTitle)
            .should("not.exist");
    });
});