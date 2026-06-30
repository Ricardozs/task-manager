export type TaskStatus = 'Pending' | 'InProgress' | 'Completed';

export interface TaskDto {
  id: string;
  title: string;
  description: string | null;
  status: TaskStatus;
  dueDate: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string | null;
  status: TaskStatus;
  dueDate: string;
}

export interface UpdateTaskRequest {
  title: string;
  description?: string | null;
  status: TaskStatus;
  dueDate: string;
}

export const TASK_STATUSES: TaskStatus[] = ['Pending', 'InProgress', 'Completed'];

export function taskStatusLabel(status: TaskStatus): string {
  switch (status) {
    case 'Pending':
      return 'Pending';
    case 'InProgress':
      return 'In progress';
    case 'Completed':
      return 'Completed';
  }
}
