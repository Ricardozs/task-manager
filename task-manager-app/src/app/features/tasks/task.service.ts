import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateTaskRequest,
  TaskDto,
  UpdateTaskRequest,
} from '../../shared/models/task.models';

@Service()
export class TaskService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getAll(): Promise<TaskDto[]> {
    return firstValueFrom(this.http.get<TaskDto[]>(`${this.apiUrl}/api/tasks`));
  }

  getById(id: string): Promise<TaskDto> {
    return firstValueFrom(this.http.get<TaskDto>(`${this.apiUrl}/api/tasks/${id}`));
  }

  create(request: CreateTaskRequest): Promise<TaskDto> {
    return firstValueFrom(this.http.post<TaskDto>(`${this.apiUrl}/api/tasks`, request));
  }

  update(id: string, request: UpdateTaskRequest): Promise<TaskDto> {
    return firstValueFrom(
      this.http.put<TaskDto>(`${this.apiUrl}/api/tasks/${id}`, request),
    );
  }

  delete(id: string): Promise<void> {
    return firstValueFrom(this.http.delete<void>(`${this.apiUrl}/api/tasks/${id}`));
  }
}
