import React from 'react'
import { MainCaroselData } from './MainCaroselData'
import 'react-alice-carousel/lib/alice-carousel.css';
import AliceCarousel from 'react-alice-carousel';

const MainCarosel = () => {
    const items = MainCaroselData.map((item) => <img className='cursor-pointer -z-10' role="presentation" src={item.image} alt="" />)
  return (
    <div>
        <AliceCarousel
            items={items}
            disableButtonsControls
            autoPlay
            autoPlayInterval={1000}
            infinite
        />
    </div>
  )
}

export default MainCarosel