export interface Task {
  id: string;
  name: string;
  checked: boolean;
  dueDate?: Date;
}

export interface TaskFormModel {
  name: string;
}

export const mockTasks: Task[] = [
  {
    id: "a",
    name: "Take out the garbage",
    checked: false,
    dueDate: new Date(2018, 10, 28, 8, 30)
  },
  {
    id: "b",
    name: "Pay the electric bill",
    checked: false,
    dueDate: new Date(2018, 11, 8, 18, 30)
  },
  {
    id: "c",
    name: "Paint the kitchen",
    checked: false
  },
  {
    id: "d",
    name: "Take wife out on date",
    checked: false
  }
];
