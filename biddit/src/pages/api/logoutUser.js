const logoutUser = () => {
    try{
        localStorage.removeItem('token');
        return true;
    }
    catch(err){
        return false;
    }
}

module.exports = logoutUser;