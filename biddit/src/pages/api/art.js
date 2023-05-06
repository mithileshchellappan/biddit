const BASE_URL = 'https://localhost:7005/api'

async function getArt(artId=0,userImages=false){
    var url;
    if(artId!==0){
      console.log('inside');
      url = `${BASE_URL}/Arts/${artId}?userImages=${userImages}`
    }else{
      console.log('inside here');
      url = `${BASE_URL}/Arts?userImages=${userImages}`
    }
    return fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token') || ''
        },
    }).then(res => {
        if(res.ok) return res.json()
        if(res.status===401){
          return false
        }else{
          throw new Error('Error getting art')
        }
        
    }
    )

}

async function uploadImage(image) {
    const url = `${BASE_URL}/Arts/uploadFile`;
    const formData = new FormData();
    formData.append('formFile', image);
  
    return fetch(url, {
      method: 'POST',
      headers: {
        'Authorization': 'Bearer ' + localStorage.getItem('token') || '',
      },
      body: formData
    }).then(async res => {
        var jsonRes = await res.json();
      if (res.ok) {
        console.log('uploaded');
        return jsonRes;
      }
      
      throw new Error('Error uploading image');
    });
  }
  

// async function uploadImage(image){
//     const url = `${BASE_URL}/Arts/uploadFile`
//     const formData = new FormData();
//     console.log('image',image);
//     formData.append('formFile', image);
//     console.log('fomr',formData.has('formFile'));
//     return fetch(url, {
//         method: 'POST',
//         headers: {
//             'Content-Type': `multipart/form-data; boundary=${formData._boundary}`,
//             'Authorization': 'Bearer ' + localStorage.getItem('token') || '',
//             'Accept-Encoding': 'gzip, deflate, br',
//             'Content-Length': ``
//         },
//         body: formData
//     }).then(async res => {

//         if(res.ok) {console.log('uploaded'); res.json()}
//         console.log(await res.json())

//         throw new Error('Error uploading image')
//     });
// }


async function addArt(art){
    const url = `${BASE_URL}/Arts`
    console.log(art);
    return fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token') || ''
        },
        body: JSON.stringify(art),
    }).then(async res => {
        if(res.ok) {var result = await res.json(); return result}
        throw new Error('Error adding art')
    }
    )
}

async function addBid(art){
  const url = `${BASE_URL}/Bids`
  console.log(art);
  return fetch(url,{
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token') || ''
  },
  body: JSON.stringify(art),

  }).then(async res => {
    if(res.ok) {var result = await res.json(); return result}
    throw new Error('Error adding bid')
  })

}


module.exports = {
    getArt,
    uploadImage,
    addArt,
    addBid
}