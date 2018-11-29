export interface TaskModel {
  id: string;
  name: string;
  ownerId: string;
  createDate: string;
  completedDate?: string;
  dueDate?: string;
}

export class Task {
  id: string;
  name: string;
  ownerId: string;
  createDate: Date;
  completedDate?: Date;
  dueDate?: Date;
  isCompleted: () => boolean;
  constructor(taskModel: TaskModel) {
    this.id = taskModel.id;
    this.name = taskModel.name;
    this.ownerId = taskModel.ownerId;
    this.createDate = new Date(taskModel.createDate);
    if (taskModel.completedDate) {
      this.completedDate = new Date(taskModel.completedDate);
    }
    if (taskModel.dueDate) {
      this.dueDate = new Date(taskModel.dueDate);
    }
    this.isCompleted = () => {
      return Boolean(this.completedDate);
    };
  }
}

export interface TaskFormModel {
  name: string;
}
