import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { environment } from '../../../environments/environment';
import { TaskDto } from '../../shared/models/task.models';
import { TaskService } from './task.service';

describe('TaskService', () => {
  let service: TaskService;
  let httpMock: HttpTestingController;

  const sampleTask: TaskDto = {
    id: '11111111-1111-1111-1111-111111111111',
    title: 'Write tests',
    description: 'Cover task CRUD',
    status: 'Pending',
    dueDate: '2026-07-01',
    createdAt: '2026-06-29T10:00:00Z',
    updatedAt: '2026-06-29T10:00:00Z',
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TaskService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(TaskService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('loads all tasks', async () => {
    const promise = service.getAll();

    const request = httpMock.expectOne(`${environment.apiUrl}/api/tasks`);
    expect(request.request.method).toBe('GET');
    request.flush([sampleTask]);

    await expect(promise).resolves.toEqual([sampleTask]);
  });

  it('creates a task', async () => {
    const promise = service.create({
      title: sampleTask.title,
      description: sampleTask.description,
      status: sampleTask.status,
      dueDate: sampleTask.dueDate,
    });

    const request = httpMock.expectOne(`${environment.apiUrl}/api/tasks`);
    expect(request.request.method).toBe('POST');
    request.flush(sampleTask);

    await expect(promise).resolves.toEqual(sampleTask);
  });

  it('updates a task', async () => {
    const promise = service.update(sampleTask.id, {
      title: 'Updated title',
      description: sampleTask.description,
      status: 'InProgress',
      dueDate: sampleTask.dueDate,
    });

    const request = httpMock.expectOne(`${environment.apiUrl}/api/tasks/${sampleTask.id}`);
    expect(request.request.method).toBe('PUT');
    request.flush({ ...sampleTask, title: 'Updated title', status: 'InProgress' });

    await expect(promise).resolves.toEqual({
      ...sampleTask,
      title: 'Updated title',
      status: 'InProgress',
    });
  });

  it('deletes a task', async () => {
    const promise = service.delete(sampleTask.id);

    const request = httpMock.expectOne(`${environment.apiUrl}/api/tasks/${sampleTask.id}`);
    expect(request.request.method).toBe('DELETE');
    request.flush(null, { status: 204, statusText: 'No Content' });

    await expect(promise).resolves.toBeNull();
  });
});
