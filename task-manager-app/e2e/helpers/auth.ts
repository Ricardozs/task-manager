import { expect, type Page } from '@playwright/test';

export const E2E_PASSWORD = 'Test1234!';

export function uniqueEmail(): string {
  return `e2e-${crypto.randomUUID()}@test.com`;
}

export function futureDate(daysFromNow = 7): string {
  const date = new Date();
  date.setDate(date.getDate() + daysFromNow);
  return date.toISOString().slice(0, 10);
}

export async function register(page: Page, email: string, password = E2E_PASSWORD): Promise<void> {
  await page.goto('/register');
  await page.getByLabel('Email').fill(email);
  await page.getByLabel('Password').fill(password);
  await page.getByRole('button', { name: 'Register' }).click();
  await expect(page).toHaveURL(/\/tasks$/);
}

export async function registerAndGoToTasks(
  page: Page,
  email: string,
  password = E2E_PASSWORD,
): Promise<void> {
  await register(page, email, password);
}

export async function login(page: Page, email: string, password = E2E_PASSWORD): Promise<void> {
  await page.goto('/login');
  await page.getByLabel('Email').fill(email);
  await page.getByLabel('Password').fill(password);
  await page.getByRole('button', { name: 'Log in' }).click();
  await expect(page).toHaveURL(/\/tasks$/);
}

export async function logout(page: Page): Promise<void> {
  await page.getByRole('button', { name: 'Log out' }).click();
  await expect(page).toHaveURL(/\/login$/);
}
