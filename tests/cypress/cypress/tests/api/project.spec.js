import { createProject } from '../../support/project';

describe('API', () => {

    after(() => {
        cy.deleteAllTestProjects();
    });

    it('responds with 404 on non-existent project', () => {
        const nonexistentProjectId = 98935110;

        cy.request({ url: `/api/project/${nonexistentProjectId}`, failOnStatusCode: false })
            .should((response) => {
                expect(response.status).to.eq(404);
            });
    });

    it('can read new empty visible project', () => {
        const projectName = `Cypress name ** ${new Date().getTime()}`;
        const projectDesc = `Cypress desc ${new Date().getTime() + 1}`;
        const projectPriority = 996999;
        const projectHidden = false;

        cy.login();

        createProject(projectName, projectDesc, projectPriority, projectHidden);

        cy.visit('/admin/project');

        cy.get('table')
            .find('tbody tr')
            .contains(projectName) // The project name table cell
            .parent('tr') // The project row
            .find('td')
            .first() // The project ID is the first cell in the row
            .as('projectId')
            .then(function () {
                cy.request(`/api/project/${this.projectId.text()}`)
                    .should((response) => {
                        expect(response.status).to.eq(200);
                        expect(response.body.id).to.eq(+this.projectId.text());
                        expect(response.body.name).to.eq(projectName);
                        expect(response.body.description).to.eq(projectDesc);
                        expect(response.body.sortOrder).to.eq(projectPriority);
                        expect(response.body.hidden).to.eq(projectHidden);
                        expect(response.body.releases).to.have.length(0);
                    });
            });
    });

    it('cannot read hidden project', () => {
        const projectName = `Cypress name ** ${new Date().getTime()}`;
        const projectDesc = `Cypress desc ${new Date().getTime() + 1}`;
        const projectPriority = 996999;
        const projectHidden = true;

        cy.login();

        createProject(projectName, projectDesc, projectPriority, projectHidden);

        cy.visit('/admin/project');

        cy.get('table')
            .find('tbody tr')
            .contains(projectName) // The project name table cell
            .parent('tr') // The project row
            .find('td')
            .first() // The project ID is the first cell in the row
            .as('projectId')
            .then(function () {
                cy.request({ url: `/api/project/${this.projectId.text()}`, failOnStatusCode: false })
                    .should((response) => {
                        expect(response.status).to.eq(404);
                    });
            });
    });
});