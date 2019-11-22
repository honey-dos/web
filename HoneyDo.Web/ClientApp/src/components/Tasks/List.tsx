import List from '@material-ui/core/List'
import React from 'react'
import { Task, TaskFormModel } from '../../lib/Task'
import TaskForm from './Form'
import TaskListItem from './ListItem'

interface TasksListProps {
  editTask?: Task
  tasks: Task[]
  onTaskUpdate: (task: Task, taskFormModel: TaskFormModel) => void
  onCancelEdit: () => void
  onDelete: (task: Task) => void
  onItemClick: (task: Task) => void
  onItemEdit: (task: Task) => void
  onCheck: (task: Task) => void
}

const TasksList = ({
  tasks,
  editTask,
  onCancelEdit,
  onCheck,
  onItemClick,
  onItemEdit,
  onTaskUpdate,
  onDelete
}: TasksListProps) => {
  const editId = editTask ? editTask.id : ''
  return (
    <List>
      {tasks.map(task =>
        task.id === editId ? (
          <TaskForm
            key={task.id}
            task={task}
            onCancel={() => onCancelEdit()}
            onSave={taskFormModel => onTaskUpdate(task, taskFormModel)}
            onDelete={() => onDelete(task)}
          />
        ) : (
          <TaskListItem
            key={task.id}
            task={task}
            onClick={() => onItemClick(task)}
            onCheck={() => onCheck(task)}
            onEdit={() => onItemEdit(task)}
          />
        )
      )}
    </List>
  )
}

export default TasksList
