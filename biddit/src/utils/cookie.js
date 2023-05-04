import Cookies from 'js-cookie'

const setToken = (token) => {
  Cookies.set('token', token, { expires: 7 })
}

const getToken = () => {
  return Cookies.get('token')
}

const removeToken = () => {
  Cookies.remove('token')
}

export {
    setToken,
    getToken,
    removeToken
}
