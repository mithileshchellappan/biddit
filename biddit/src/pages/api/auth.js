const BASE_URL = 'https://localhost:7005/api'

async function login(username,password){
    console.log(username,password);
    const url = `${BASE_URL}/Users/login`
    return fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            userName: username,
            password
        })
    }).then(async res => {
        if(res.ok) {
            console.log('inside');
            var resJson = await res.json();
            localStorage.setItem('token',resJson.jwtToken);
            return resJson
        }
        return null
    }
    )

}

async function signUp(username,password){
    const url = `${BASE_URL}/Users`
    return fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            userName: username,
            password
        })
    }).then(async res => {
        if(res.ok) {
            var resJson = await res.json();
            localStorage.setItem('token',resJson.jwtToken);
            return resJson
        }
        console.log(await res.json())
        return null
    }
    )
}

export {login,signUp}