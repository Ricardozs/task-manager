import { expect, test } from '@playwright/test';

import { E2E_PASSWORD, login, logout, register, uniqueEmail } from './helpers/auth';

test.describe('Authentication', () => {
  test('redirects unauthenticated users from /tasks to /login', async ({ page }) => {
    await page.goto('/tasks');
    await expect(page).toHaveURL(/\/login$/);
    await expect(page.getByRole('heading', { name: 'Log in' })).toBeVisible();
  });

  test('registers a new user and lands on /tasks', async ({ page }) => {
    const email = uniqueEmail();

    await register(page, email);

    await expect(page.getByRole('heading', { name: 'Tasks', exact: true })).toBeVisible();
    await expect(page.getByText(`Signed in as ${email}`)).toBeVisible();
  });

  test('logs in after logging out', async ({ page }) => {
    const email = uniqueEmail();

    await register(page, email);
    await logout(page);
    await login(page, email);

    await expect(page.getByRole('heading', { name: 'Tasks', exact: true })).toBeVisible();
    await expect(page.getByText(`Signed in as ${email}`)).toBeVisible();
  });

  test('shows an error for invalid credentials', async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('not-a-user@example.com');
    await page.getByLabel('Password').fill(E2E_PASSWORD);
    await page.getByRole('button', { name: 'Log in' }).click();

    await expect(page.getByRole('alert')).toHaveText('Invalid email or password.');
    await expect(page).toHaveURL(/\/login$/);
  });
});
