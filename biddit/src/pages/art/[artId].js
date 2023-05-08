import Image from 'next/image';
import { useState, useEffect } from 'react';
import { useRouter } from 'next/router';
import { getArt, addUserBid } from '../api/art';
import Link from 'next/link';

export default function ArtView() {
  const [currentBid, setCurrentBid] = useState({});
  const [newBid, setNewBid] = useState(currentBid);
  const [art, setArt] = useState({});
  const [bidData, setBidData] = useState([]);
  const router = useRouter();

  const handleBidChange = (event) => {
    setNewBid(event.target.value);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setCurrentBid(newBid);
    if (newBid <= currentBid.bidAmount) {
      console.log(newBid,currentBid.bidAmount);
      alert('Your bid must be higher than the current bid!');
      setCurrentBid(currentBid);
      return;
    }else{
      var bidObj = {
        BidId :art.bidId,
        BidAmount : parseInt(newBid)
      }

     var res =  await addUserBid(bidObj)
     if(res){
      router.reload();
     }else{
      alert('Something went wrong while adding your bid')
     }

    }
    setNewBid(currentBid);
  };

  useEffect(() => {
    const { artId } = router.query;
    if (artId) {
      const fetchArt = async () => {
        const token = localStorage.getItem('token');
        if (token || token !== 'null') {
          const artData = await getArt(artId);
          console.log(artData)
          artData.art.userName = artData.userName
          artData.art.BidCount = artData.userBids.length
          setArt(artData.art);
          setBidData(artData.userBids)
          let highestBid = {bidAmount: 0, userId: 0};   
          if(artData.userBids.length > 0 && artData.userBids[0]!==null) {
            for (let i = 0; i < artData.userBids.length; i++) {
              if (artData.userBids[i].bidAmount > highestBid.bidAmount) {
                highestBid = artData.userBids[i];
              }
            }
          }else{
            console.log("no bids yet");
          }
          console.log(highestBid);
          setCurrentBid(highestBid);
        } else {
          router.push('/login');
        }
      };
      fetchArt();
    }
  }, [router.query]);

  return (
    <div className="flex flex-col h-screen">
      <nav className="flex justify-between items-center bg-white py-4 px-8 shadow">
        <div className="flex items-center">
          <Link href="/explore">
            <div className="text-xl font-bold text-gray-800 hover:text-gray-600 mr-4">Explore</div>
          </Link>
          <Link href="/my-art">
            <div className="text-xl font-bold text-gray-800 hover:text-gray-600 mr-4">My Art</div>
          </Link>
        </div>
        <Link href="/createArt">
          <div className="bg-indigo-600 hover:bg-indigo-700 text-white py-2 px-4 rounded">
            Create Art
          </div>
        </Link>
      </nav>
    <div className="flex flex-row gap-8 bg-gray-200 p-8">
      <div className="w-1/2 border border-gray-400 rounded-md overflow-hidden shadow-md">
        <Image
          src={art.artURL}
          width={2048}
          height={1365}
          unoptimized
        />
      </div>
      <div className="w-1/2">
        <h1 className="text-5xl font-bold mb-4">{art.title}</h1>
        <p className="text-lg mb-4">Description: {art.description}</p>
        <p className="text-lg mb-4">Owner: {art.userName}</p>
        {currentBid.userId!==0?<p className="text-lg mb-4">Current highest bid: <b>${currentBid.bidAmount}</b> by <i>user {currentBid.userId}</i></p>:<p className="text-lg mb-4">No bids yet</p>}
        <p>Total Bids: {art.BidCount}</p>
        <form onSubmit={handleSubmit}>
          <label htmlFor="bidAmount" className="block text-lg mb-2">
            Enter your bid:
          </label>
          <div className="flex flex-row items-center">
            <input
              id="bidAmount"
              name="bidAmount"
              type="number"
              value={newBid}
              onChange={handleBidChange}
              className="mr-2 p-2 w-32 border border-gray-400 rounded-lg"
              required
            />
            <button type="submit" className="px-4 py-2 bg-blue-500 text-white rounded-lg">
              Place Bid
            </button>
          </div>
        </form>
      </div>
    </div>
    </div>
  );
}
