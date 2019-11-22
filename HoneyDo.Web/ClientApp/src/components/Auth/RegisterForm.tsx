import React, { Component } from 'react'

interface RegisterFormProps {
  onSubmit: (name: string, userName: string) => void
  onCancel: () => void
}

interface RegisterFormState {
  name: string
  userName: string
}

const initialState: RegisterFormState = {
  name: '',
  userName: ''
}

class RegisterForm extends Component<RegisterFormProps, RegisterFormState> {
  constructor(props: RegisterFormProps) {
    super(props)
    this.state = initialState
  }
  nameChange = (name: string) => {
    this.setState({ name })
  }
  userNameChange = (userName: string) => {
    this.setState({ userName })
  }
  handleSubmit = () => {
    const { name, userName } = this.state
    if (!name || !name.length || !userName || !userName.length) {
      return
    }
    this.props.onSubmit(name, userName)
  }
  render() {
    return (
      <form onSubmit={this.handleSubmit}>
        <label>
          Name:
          <input
            type="text"
            value={this.state.name}
            onChange={event => this.nameChange(event.target.value)}
          />
        </label>
        <label>
          UserName:
          <input
            type="text"
            value={this.state.userName}
            onChange={event => this.userNameChange(event.target.value)}
          />
        </label>
        <button onClick={this.props.onCancel} type="button">
          Cancel
        </button>
        <button type="submit">Register</button>
      </form>
    )
  }
}

export default RegisterForm
