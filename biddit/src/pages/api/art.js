const BASE_URL = 'https://localhost:7005/api'

async function getArt(){
    const url = `${BASE_URL}/Arts`
    return fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    }).then(res => {
        if(res.ok) return res.json()
        throw new Error('Error fetching art')
    }
    )

}

async function uploadImage(image){
    const url = `${BASE_URL}/Arts/uploadFile`
    const formData = new FormData();
    console.log(image);
    formData.append('formFile', image);
    return fetch(url, {
        method: 'POST',
        body: formData
    }).then(res => {
        console.log(res);
        if(res.ok) return res.json()
        throw new Error('Error uploading image')
    });
}


async function addArt(art){
    const url = `${BASE_URL}/Arts`
    console.log(art);
    return fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token') || ''
        },
        body: art,
    }).then(res => {
        if(res.ok) return res.json()
        throw new Error('Error adding art')
    }
    )
}

module.exports = {
    getArt,
    uploadImage,
    addArt
}