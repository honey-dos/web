import React, { Component, MouseEvent } from 'react'
import { withStyles } from '@material-ui/core/styles'
import AppBar from '@material-ui/core/AppBar'
import Toolbar from '@material-ui/core/Toolbar'
import Typography from '@material-ui/core/Typography'
import IconButton from '@material-ui/core/IconButton'
import Button from '@material-ui/core/Button'
import { Menu as MenuIcon, Settings } from '@material-ui/icons'
import MenuItem from '@material-ui/core/MenuItem'
import Menu from '@material-ui/core/Menu'
import { Link } from 'react-router-dom'
import { Hidden } from '@material-ui/core'
import { UserContext, UserContextData } from '../../contexts/UserContext'

const styles = {
  root: {
    display: 'block'
  },
  flex: {
    flex: 1
  },
  menuButton: {
    marginLeft: -12,
    marginRight: 20
  },
  toolbarContainer: {
    maxWidth: 800,
    marginRight: 'auto',
    marginLeft: 'auto'
  }
}

interface Props {
  classes: { [key: string]: any }
  toggleDrawer: () => void
}

interface State {
  anchorEl: HTMLElement | null
}

const initialState: State = {
  anchorEl: null
}

class PrimaryAppBar extends Component<Props, State> {
  static contextType = UserContext

  constructor(props: any) {
    super(props)
    this.state = initialState
  }

  handleMenu = (event: MouseEvent<HTMLElement>) => {
    this.setState({ anchorEl: event.currentTarget })
  }

  logout = () => {
    const { logout }: UserContextData = this.context
    logout()
    this.handleClose()
  }

  handleClose = () => {
    this.setState({ anchorEl: null })
  }

  render() {
    const { isLoggedIn }: UserContextData = this.context
    const { classes, toggleDrawer } = this.props
    const { anchorEl } = this.state
    const open = Boolean(anchorEl)

    return (
      <AppBar position="static" className={classes.root}>
        <div className={classes.toolbarContainer}>
          <Toolbar variant="dense">
            {/* Right now the follow <Hidden> will never show,  I'm leaving it here for when we have a menu, and we'll ad it back in. */}
            <Hidden mdUp={true} mdDown={true}>
              <IconButton
                onClick={() => toggleDrawer()}
                className={classes.menuButton}
                color="inherit"
                aria-label="Menu">
                <MenuIcon />
              </IconButton>
            </Hidden>
            <Link to={'/'} className={classes.flex}>
              <Typography variant="h5" color="inherit">
                Honey-Dos
              </Typography>
            </Link>
            {isLoggedIn() ? (
              <div>
                <IconButton
                  aria-owns={open ? 'menu-appbar' : undefined}
                  aria-haspopup="true"
                  onClick={this.handleMenu}
                  color="inherit">
                  <Settings />
                </IconButton>
                <Menu
                  id="menu-appbar"
                  anchorEl={anchorEl}
                  anchorOrigin={{
                    vertical: 'top',
                    horizontal: 'right'
                  }}
                  transformOrigin={{
                    vertical: 'top',
                    horizontal: 'right'
                  }}
                  open={open}
                  onClose={this.handleClose}>
                  <Link to={'/'}>
                    <MenuItem onClick={this.handleClose}>Home</MenuItem>
                  </Link>
                  <Link to={'/tasks'}>
                    <MenuItem onClick={this.handleClose}>Tasks</MenuItem>
                  </Link>
                  <Link to={'/login'}>
                    <MenuItem onClick={this.handleClose}>Login Page</MenuItem>
                  </Link>
                  <Link to={'/login'}>
                    <MenuItem onClick={this.logout}>Logout</MenuItem>
                  </Link>
                </Menu>
              </div>
            ) : (
              <div>
                <Link to={'/login'}>
                  <Button color="inherit">Login</Button>
                </Link>
                <Link to={'/login'}>
                  <Button color="inherit">Sign Up</Button>
                </Link>
              </div>
            )}
          </Toolbar>
        </div>
      </AppBar>
    )
  }
}

export default withStyles(styles)(PrimaryAppBar)
