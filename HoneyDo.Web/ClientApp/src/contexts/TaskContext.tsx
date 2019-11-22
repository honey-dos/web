import React, { Component } from 'react'
import { Task, TaskModel, TaskFormModel } from '../lib/Task'
import { FetchContext, IFetch } from './FetchContext'

export interface IGetTasks {
  (): Promise<Task[]>
}

export interface ICreateTask {
  (taskFormModel: TaskFormModel): Promise<Task | null>
}

export interface ICompleteTask {
  (task: Task, isCompleted: boolean): Promise<Task | null>
}

export interface IUpdateTask {
  (task: Task, taskFormModel: TaskFormModel): Promise<Task | null>
}

export interface IDeleteTask {
  (task: Task): Promise<boolean>
}

export interface TaskContextData {
  getTasks: IGetTasks
  createTask: ICreateTask
  completeTask: ICompleteTask
  updateTask: IUpdateTask
  deleteTask: IDeleteTask
}

export const TaskContext = React.createContext<TaskContextData>({
  getTasks: async () => [],
  createTask: async () => null,
  completeTask: async () => null,
  updateTask: async () => null,
  deleteTask: async () => false
})

const { Provider, Consumer } = TaskContext

export class TaskProvider extends Component {
  static contextType = FetchContext

  getTasks: IGetTasks = async (): Promise<Task[]> => {
    const { fetch }: { fetch: IFetch } = this.context
    const url = `api/todos/`
    const taskRequest = await fetch(url, {
      method: 'GET',
      cache: 'no-cache'
    })
    const taskModels: TaskModel[] = await taskRequest.json()
    const tasks = taskModels.map(i => new Task(i))
    return tasks
  }

  createTask: ICreateTask = async (
    taskFormModel: TaskFormModel
  ): Promise<Task> => {
    const { fetch }: { fetch: IFetch } = this.context
    const url = `api/todos/`
    const taskRequest = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json; charset=utf-8'
      },
      body: JSON.stringify(taskFormModel)
    })
    const taskModel: TaskModel = await taskRequest.json()
    return new Task(taskModel)
  }

  completeTask: ICompleteTask = async (
    task: Task,
    isCompleted: boolean
  ): Promise<Task> => {
    const { fetch }: { fetch: IFetch } = this.context
    const url = `api/todos/${task.id}/complete`
    const method = isCompleted ? 'PUT' : 'DELETE'
    const taskRequest = await fetch(url, {
      method
    })
    const taskModel: TaskModel = await taskRequest.json()
    return new Task(taskModel)
  }

  updateTask: IUpdateTask = async (
    task: Task,
    taskFormModel: TaskFormModel
  ) => {
    const { fetch }: { fetch: IFetch } = this.context
    const url = `api/todos/${task.id}`
    const taskRequest = await fetch(url, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json; charset=utf-8'
      },
      body: JSON.stringify(taskFormModel)
    })
    const taskModel: TaskModel = await taskRequest.json()
    return new Task(taskModel)
  }

  deleteTask: IDeleteTask = async (task: Task) => {
    const { fetch }: { fetch: IFetch } = this.context
    const url = `api/todos/${task.id}`
    const taskRequest = await fetch(url, {
      method: 'Delete',
      headers: {
        'Content-Type': 'application/json; charset=utf-8'
      }
    })
    return taskRequest.status === 204
  }

  render() {
    const { children } = this.props

    return (
      <Provider
        value={{
          getTasks: () => this.getTasks(),
          createTask: taskFormModel => this.createTask(taskFormModel),
          completeTask: (task, isCompleted) =>
            this.completeTask(task, isCompleted),
          updateTask: (task, taskFormModel) =>
            this.updateTask(task, taskFormModel),
          deleteTask: task => this.deleteTask(task)
        }}>
        {children}
      </Provider>
    )
  }
}

export const TaskConsumer = Consumer
