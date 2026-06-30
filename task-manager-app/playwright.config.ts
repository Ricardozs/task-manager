import { defineConfig, devices } from '@playwright/test';

const isCi = !!process.env['CI'];
const angularManaged = !!process.env['PLAYWRIGHT_TEST_BASE_URL'];

export default defineConfig({
  testDir: './e2e',
  fullyParallel: true,
  forbidOnly: isCi,
  retries: isCi ? 2 : 0,
  workers: isCi ? 1 : undefined,
  reporter: 'html',
  use: {
    baseURL: process.env['PLAYWRIGHT_TEST_BASE_URL'] ?? 'http://localhost:4200',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
  webServer: [
    {
      command: 'dotnet run --project src/TaskManager.Api --launch-profile http',
      url: 'http://localhost:5221/swagger/index.html',
      reuseExistingServer: !isCi,
      timeout: 120_000,
      cwd: '../task-manager-api',
    },
    ...(angularManaged
      ? []
      : [
          {
            command: 'npx ng serve',
            url: 'http://localhost:4200',
            reuseExistingServer: !isCi,
            timeout: 120_000,
          },
        ]),
  ],
});
