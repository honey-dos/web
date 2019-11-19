import React, { useState, useRef, useEffect, FunctionComponent } from 'react'
import {
  IconButton,
  Button,
  TextField,
  Theme,
  createStyles,
  withStyles
} from '@material-ui/core'
import { AddCircleOutline } from '@material-ui/icons'
import { KeyboardDateTimePicker } from '@material-ui/pickers'
import { Task, TaskFormModel } from '../../lib/Task'

const styles = ({ spacing }: Theme) =>
  createStyles({
    button: {
      margin: spacing(),
      float: 'right'
    },
    inputContainer: {
      display: 'flex',
      flexWrap: 'wrap'
    },
    input: {
      margin: spacing()
    },
    dateInput: {},
    addDueDate: {
      margin: spacing(),
      flexBasis: 160
    },
    dueDateContainer: {
      width: '100%',
      display: 'flex'
    },
    dueDateButtonIcon: {
      flex: '0 0 auto'
    },
    dueDateIcon: {
      transform: 'rotate(45deg)'
    },
    buttonContainer: {
      display: 'flex',
      flexDirection: 'row-reverse'
    }
  })

interface TaskFormProps {
  task?: Task
  classes: { [key: string]: string }
  onSave: (task: TaskFormModel) => void
  onCancel: () => void
  onDelete?: () => void
}

const useComponentDidMount = (func: () => void) => {
  const [hasMounted, setMounted] = useState(false)
  useEffect(() => {
    if (!hasMounted && func) {
      setMounted(true)
      func()
    }
  }, [hasMounted, func])
}

const TaskFormFunction: FunctionComponent<TaskFormProps> = (
  props: TaskFormProps
) => {
  const { classes, task, onCancel, onDelete, onSave } = props
  const [name, updateName] = useState((props.task && props.task.name) || '')
  const [dueDate, updateDueDate] = useState<Date | undefined>(
    (props.task && props.task.dueDate) || undefined
  )
  const [hasError, updateError] = useState(false)
  const nameRef = useRef({ focus: () => {} })
  useComponentDidMount(() => {
    nameRef && nameRef.current && nameRef.current.focus()
  })
  const handleSave = () => {
    if (!name || name.trim().length === 0) {
      updateError(true)
      return
    }
    const newTask: TaskFormModel = {
      name,
      dueDate
    }
    onSave(newTask)
  }

  return (
    <div>
      <div className={classes.inputContainer}>
        <TextField
          id="name"
          label="Name"
          value={name}
          onChange={event => updateName(event.target.value)}
          // onKeyUp={this.saveOnEnter()}
          margin="dense"
          variant="outlined"
          fullWidth
          inputRef={nameRef}
          error={hasError}
          helperText={hasError ? 'Name is required.' : null}
          className={classes.input}
        />
        {dueDate ? (
          <div className={classes.dueDateContainer}>
            <KeyboardDateTimePicker
              disablePast
              variant="inline"
              inputVariant="outlined"
              format="yyyy/MM/dd HH:mm"
              margin="dense"
              className={[classes.input, classes.dateInput].join(' ')}
              fullWidth
              label="Due Date"
              value={dueDate}
              onChange={date => updateDueDate(date || undefined)}
              KeyboardButtonProps={{
                'aria-label': 'change date'
              }}
              // onKeyUp={this.saveOnEnter()}
            />
            <IconButton
              className={classes.dueDateButtonIcon}
              aria-label="remove due date"
              onClick={() => updateDueDate(undefined)}>
              <AddCircleOutline className={classes.dueDateIcon} />
            </IconButton>
          </div>
        ) : (
          <Button
            className={classes.addDueDate}
            onClick={() => updateDueDate(new Date())}>
            Add Due Date
          </Button>
        )}
      </div>
      <div className={classes.buttonContainer}>
        <Button className={classes.button} onClick={() => handleSave()}>
          Save
        </Button>
        <Button className={classes.button} onClick={() => onCancel()}>
          Cancel
        </Button>
        {task ? (
          <Button
            className={classes.button}
            onClick={() => (onDelete ? onDelete() : null)}>
            Delete
          </Button>
        ) : null}
      </div>
    </div>
  )
}

export default withStyles(styles)(TaskFormFunction)
