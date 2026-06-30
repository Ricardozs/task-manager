import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../../core/auth/auth.service';
import {
  TaskDto,
  TASK_STATUSES,
  taskStatusLabel,
  TaskStatus,
} from '../../../shared/models/task.models';
import { getApiErrorMessage } from '../../../shared/utils/api-error';
import { TaskForm } from '../task-form/task-form';
import { TaskService } from '../task.service';

@Component({
  selector: 'app-task-list',
  imports: [TaskForm],
  template: `
    <div class="tasks-page">
      <header class="tasks-header">
        <div>
          <h1>Tasks</h1>
          @if (authService.email()) {
            <p class="tasks-subtitle">Signed in as {{ authService.email() }}</p>
          }
        </div>
        <button type="button" class="btn-secondary" (click)="logout()">Log out</button>
      </header>

      @if (loadError()) {
        <p class="form-error" role="alert">{{ loadError() }}</p>
      }

      <section class="tasks-card" aria-labelledby="create-task-heading">
        <app-task-form mode="create" (saved)="onTaskCreated($event)" />
      </section>

      <section class="tasks-card" aria-labelledby="task-list-heading">
        <div class="task-list-header">
          <h2 id="task-list-heading">Your tasks</h2>
          @if (loading()) {
            <p class="tasks-status" aria-live="polite">Loading tasks…</p>
          }
        </div>

        @if (!loading() && tasks().length === 0) {
          <p class="tasks-empty">No tasks yet. Create your first task above.</p>
        }

        <ul class="task-list" aria-label="Task list">
          @for (task of sortedTasks(); track task.id) {
            <li class="task-item">
              @if (editingTaskId() === task.id) {
                <app-task-form
                  mode="edit"
                  [task]="task"
                  [showCancel]="true"
                  (saved)="onTaskUpdated($event)"
                  (cancelled)="stopEditing()"
                />
              } @else {
                <article class="task-row" [attr.aria-label]="task.title">
                  <div class="task-main">
                    <h3>{{ task.title }}</h3>
                    @if (task.description) {
                      <p class="task-description">{{ task.description }}</p>
                    }
                    <p class="task-meta">
                      Due {{ task.dueDate }}
                      <span class="task-status-badge" [class]="statusClass(task.status)">
                        {{ taskStatusLabel(task.status) }}
                      </span>
                    </p>
                  </div>

                  <div class="task-actions">
                    <label class="sr-only" [for]="'status-' + task.id">Status</label>
                    @if (task.status === 'Completed') {
                      <span class="status-readonly">{{ taskStatusLabel(task.status) }}</span>
                    } @else {
                      <select
                        [id]="'status-' + task.id"
                        class="status-select"
                        [value]="task.status"
                        [disabled]="statusUpdatingId() === task.id"
                        (change)="onStatusChange(task, $event)"
                      >
                        @for (status of statuses; track status) {
                          <option [value]="status">{{ taskStatusLabel(status) }}</option>
                        }
                      </select>
                    }

                    <button
                      type="button"
                      class="btn-secondary"
                      (click)="startEditing(task.id)"
                    >
                      Edit
                    </button>
                    <button
                      type="button"
                      class="btn-danger"
                      [disabled]="deletingId() === task.id"
                      (click)="deleteTask(task)"
                    >
                      {{ deletingId() === task.id ? 'Deleting…' : 'Delete' }}
                    </button>
                  </div>

                  @if (actionErrorId() === task.id) {
                    <p class="form-error task-action-error" role="alert">
                      {{ actionError() }}
                    </p>
                  }
                </article>
              }
            </li>
          }
        </ul>
      </section>
    </div>
  `,
})
export class TaskList implements OnInit {
  protected readonly authService = inject(AuthService);
  private readonly taskService = inject(TaskService);
  private readonly router = inject(Router);

  protected readonly tasks = signal<TaskDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly loadError = signal<string | null>(null);
  protected readonly editingTaskId = signal<string | null>(null);
  protected readonly deletingId = signal<string | null>(null);
  protected readonly statusUpdatingId = signal<string | null>(null);
  protected readonly actionError = signal<string | null>(null);
  protected readonly actionErrorId = signal<string | null>(null);

  protected readonly statuses = TASK_STATUSES;
  protected readonly taskStatusLabel = taskStatusLabel;

  protected readonly sortedTasks = computed(() =>
    [...this.tasks()].sort((left, right) => left.dueDate.localeCompare(right.dueDate)),
  );

  async ngOnInit(): Promise<void> {
    await this.loadTasks();
  }

  protected statusClass(status: TaskStatus): string {
    return `status-${status.toLowerCase()}`;
  }

  protected async loadTasks(): Promise<void> {
    this.loading.set(true);
    this.loadError.set(null);

    try {
      const tasks = await this.taskService.getAll();
      this.tasks.set(tasks);
    } catch (error) {
      this.loadError.set(getApiErrorMessage(error));
    } finally {
      this.loading.set(false);
    }
  }

  protected onTaskCreated(task: TaskDto): void {
    this.tasks.update((current) => [...current, task]);
    this.clearActionError();
  }

  protected onTaskUpdated(task: TaskDto): void {
    this.tasks.update((current) =>
      current.map((existing) => (existing.id === task.id ? task : existing)),
    );
    this.stopEditing();
    this.clearActionError();
  }

  protected startEditing(taskId: string): void {
    this.editingTaskId.set(taskId);
    this.clearActionError();
  }

  protected stopEditing(): void {
    this.editingTaskId.set(null);
  }

  protected async onStatusChange(task: TaskDto, event: Event): Promise<void> {
    const select = event.target as HTMLSelectElement;
    const newStatus = select.value as TaskStatus;

    if (newStatus === task.status || task.status === 'Completed') {
      return;
    }

    this.statusUpdatingId.set(task.id);
    this.clearActionError();

    try {
      const updated = await this.taskService.update(task.id, {
        title: task.title,
        description: task.description,
        status: newStatus,
        dueDate: task.dueDate,
      });
      this.tasks.update((current) =>
        current.map((existing) => (existing.id === updated.id ? updated : existing)),
      );
    } catch (error) {
      select.value = task.status;
      this.setActionError(task.id, getApiErrorMessage(error));
    } finally {
      this.statusUpdatingId.set(null);
    }
  }

  protected async deleteTask(task: TaskDto): Promise<void> {
    const confirmed = globalThis.confirm(`Delete "${task.title}"?`);
    if (!confirmed) {
      return;
    }

    this.deletingId.set(task.id);
    this.clearActionError();

    try {
      await this.taskService.delete(task.id);
      this.tasks.update((current) => current.filter((existing) => existing.id !== task.id));

      if (this.editingTaskId() === task.id) {
        this.stopEditing();
      }
    } catch (error) {
      this.setActionError(task.id, getApiErrorMessage(error));
    } finally {
      this.deletingId.set(null);
    }
  }

  protected logout(): void {
    this.authService.logout();
    void this.router.navigateByUrl('/login');
  }

  private setActionError(taskId: string, message: string): void {
    this.actionErrorId.set(taskId);
    this.actionError.set(message);
  }

  private clearActionError(): void {
    this.actionErrorId.set(null);
    this.actionError.set(null);
  }
}
