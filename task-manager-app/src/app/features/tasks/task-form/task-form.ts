import { Component, effect, inject, input, output, signal } from '@angular/core';
import { form, FormField, required, submit } from '@angular/forms/signals';

import { TaskService } from '../task.service';
import {
  CreateTaskRequest,
  TaskDto,
  TASK_STATUSES,
  taskStatusLabel,
  TaskStatus,
  UpdateTaskRequest,
} from '../../../shared/models/task.models';
import { getApiErrorMessage } from '../../../shared/utils/api-error';

type TaskFormModel = {
  title: string;
  description: string;
  status: TaskStatus;
  dueDate: string;
};

function todayIsoDate(): string {
  return new Date().toISOString().slice(0, 10);
}

function emptyFormModel(): TaskFormModel {
  return {
    title: '',
    description: '',
    status: 'Pending',
    dueDate: todayIsoDate(),
  };
}

function toFormModel(task: TaskDto): TaskFormModel {
  return {
    title: task.title,
    description: task.description ?? '',
    status: task.status,
    dueDate: task.dueDate,
  };
}

@Component({
  selector: 'app-task-form',
  imports: [FormField],
  template: `
    <form class="task-form" (submit)="onSubmit($event)" novalidate>
      <h2 [id]="headingId()">{{ heading() }}</h2>

      @if (apiError()) {
        <p class="form-error" role="alert">{{ apiError() }}</p>
      }

      <div class="form-field">
        <label [for]="fieldId('title')">Title</label>
        <input
          [id]="fieldId('title')"
          type="text"
          autocomplete="off"
          [formField]="taskForm.title"
        />
        @if (taskForm.title().touched() && taskForm.title().invalid()) {
          <ul class="field-errors">
            @for (error of taskForm.title().errors(); track error.kind) {
              <li>{{ error.message }}</li>
            }
          </ul>
        }
      </div>

      <div class="form-field">
        <label [for]="fieldId('description')">Description</label>
        <textarea
          [id]="fieldId('description')"
          rows="3"
          [formField]="taskForm.description"
        ></textarea>
      </div>

      <div class="form-row">
        <div class="form-field">
          <label [for]="fieldId('status')">Status</label>
          @if (isStatusLocked()) {
            <p [id]="fieldId('status')" class="status-readonly">
              {{ taskStatusLabel(formModel().status) }}
            </p>
          } @else {
            <select [id]="fieldId('status')" [formField]="taskForm.status">
              @for (status of statuses; track status) {
                <option [value]="status">{{ taskStatusLabel(status) }}</option>
              }
            </select>
          }
        </div>

        <div class="form-field">
          <label [for]="fieldId('dueDate')">Due date</label>
          <input
            [id]="fieldId('dueDate')"
            type="date"
            [attr.min]="minDueDate()"
            [formField]="taskForm.dueDate"
          />
          @if (taskForm.dueDate().touched() && taskForm.dueDate().invalid()) {
            <ul class="field-errors">
              @for (error of taskForm.dueDate().errors(); track error.kind) {
                <li>{{ error.message }}</li>
              }
            </ul>
          }
        </div>
      </div>

      <div class="form-actions">
        <button type="submit" class="btn-primary" [disabled]="submitting()">
          {{ submitting() ? 'Saving…' : submitLabel() }}
        </button>
        @if (showCancel()) {
          <button type="button" class="btn-secondary" (click)="cancelled.emit()">
            Cancel
          </button>
        }
      </div>
    </form>
  `,
})
export class TaskForm {
  private readonly taskService = inject(TaskService);

  readonly mode = input.required<'create' | 'edit'>();
  readonly task = input<TaskDto | null>(null);
  readonly showCancel = input(false);

  readonly saved = output<TaskDto>();
  readonly cancelled = output<void>();

  protected readonly statuses = TASK_STATUSES;
  protected readonly taskStatusLabel = taskStatusLabel;
  protected readonly submitting = signal(false);
  protected readonly apiError = signal<string | null>(null);

  protected readonly formModel = signal<TaskFormModel>(emptyFormModel());

  protected readonly taskForm = form(this.formModel, (schemaPath) => {
    required(schemaPath.title, { message: 'Title is required' });
    required(schemaPath.dueDate, { message: 'Due date is required' });
  });

  protected readonly heading = () =>
    this.mode() === 'create' ? 'New task' : 'Edit task';

  protected readonly submitLabel = () => (this.mode() === 'create' ? 'Create task' : 'Save changes');

  protected readonly headingId = () =>
    this.mode() === 'create' ? 'create-task-heading' : 'edit-task-heading';

  protected readonly isStatusLocked = () =>
    this.mode() === 'edit' && this.task()?.status === 'Completed';

  protected readonly minDueDate = () => (this.mode() === 'create' ? todayIsoDate() : null);

  constructor() {
    effect(() => {
      const task = this.task();
      if (this.mode() === 'edit' && task) {
        this.formModel.set(toFormModel(task));
      } else if (this.mode() === 'create') {
        this.formModel.set(emptyFormModel());
      }
      this.apiError.set(null);
    });
  }

  protected fieldId(name: string): string {
    return `${this.mode()}-task-${name}`;
  }

  protected async onSubmit(event: Event): Promise<void> {
    event.preventDefault();
    this.apiError.set(null);
    this.submitting.set(true);

    try {
      await submit(this.taskForm, async () => {
        const model = this.formModel();

        if (this.mode() === 'create' && model.dueDate < todayIsoDate()) {
          this.apiError.set('Due date cannot be in the past.');
          return;
        }

        try {
          const payload: CreateTaskRequest | UpdateTaskRequest = {
            title: model.title.trim(),
            description: model.description.trim() || null,
            status: model.status,
            dueDate: model.dueDate,
          };

          const savedTask =
            this.mode() === 'create'
              ? await this.taskService.create(payload)
              : await this.taskService.update(this.task()!.id, payload);

          if (this.mode() === 'create') {
            this.formModel.set(emptyFormModel());
          }

          this.saved.emit(savedTask);
        } catch (error) {
          this.apiError.set(getApiErrorMessage(error));
        }
      });
    } finally {
      this.submitting.set(false);
    }
  }
}
