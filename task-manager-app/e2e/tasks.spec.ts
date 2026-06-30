import { expect, test } from '@playwright/test';

import { futureDate, registerAndGoToTasks, uniqueEmail } from './helpers/auth';

test.describe('Tasks', () => {
  test.beforeEach(async ({ page }) => {
    await registerAndGoToTasks(page, uniqueEmail());
  });

  test('shows an empty task list after registration', async ({ page }) => {
    await expect(page.getByText('No tasks yet. Create your first task above.')).toBeVisible();
  });

  test('creates a task', async ({ page }) => {
    const title = 'E2E created task';
    const dueDate = futureDate();

    await page.locator('#create-task-title').fill(title);
    await page.locator('#create-task-dueDate').fill(dueDate);
    await page.getByRole('button', { name: 'Create task' }).click();

    const task = page.getByRole('article', { name: title });
    await expect(task).toBeVisible();
    await expect(task).toContainText(dueDate);
    await expect(task).toContainText('Pending');
  });

  test('edits a task title', async ({ page }) => {
    const originalTitle = 'Task to edit';
    const updatedTitle = 'Updated task title';
    const dueDate = futureDate();

    await page.locator('#create-task-title').fill(originalTitle);
    await page.locator('#create-task-dueDate').fill(dueDate);
    await page.getByRole('button', { name: 'Create task' }).click();

    const task = page.getByRole('article', { name: originalTitle });
    await task.getByRole('button', { name: 'Edit' }).click();

    await page.locator('#edit-task-title').fill(updatedTitle);
    await page.getByRole('button', { name: 'Save changes' }).click();

    await expect(page.getByRole('article', { name: updatedTitle })).toBeVisible();
    await expect(page.getByRole('article', { name: originalTitle })).toHaveCount(0);
  });

  test('changes task status', async ({ page }) => {
    const title = 'Task to update status';
    const dueDate = futureDate();

    await page.locator('#create-task-title').fill(title);
    await page.locator('#create-task-dueDate').fill(dueDate);
    await page.getByRole('button', { name: 'Create task' }).click();

    const task = page.getByRole('article', { name: title });
    await task.locator('select.status-select').selectOption('InProgress');

    await expect(task).toContainText('In progress');
  });

  test('deletes a task', async ({ page }) => {
    const title = 'Task to delete';
    const dueDate = futureDate();

    await page.locator('#create-task-title').fill(title);
    await page.locator('#create-task-dueDate').fill(dueDate);
    await page.getByRole('button', { name: 'Create task' }).click();

    const task = page.getByRole('article', { name: title });
    await expect(task).toBeVisible();

    page.on('dialog', (dialog) => dialog.accept());
    await task.getByRole('button', { name: 'Delete' }).click();

    await expect(page.getByRole('article', { name: title })).toHaveCount(0);
  });
});
