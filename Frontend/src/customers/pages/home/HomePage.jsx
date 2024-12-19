import React from 'react';
import MainCarosel from '../../components/homeCarousal/MainCarosel';
import HomeSectionCarousel from '../../components/homeSectionCarousel/HomeSectionCarousel';
import {men_kurta} from "../../../Data/men_kurta.js"

const HomePage = () => {
    return (
        <div>
            <MainCarosel />

            <div className="space-y-10 py-20 flex flex-col justify-center px-5 lg:px-10">
                <HomeSectionCarousel data={men_kurta} sectionName={"Men's Kurtas"} />
                <HomeSectionCarousel data={men_kurta} sectionName={"Men's Shoes"}  />
                <HomeSectionCarousel data={men_kurta} sectionName={"Men's Shirt"}  />
                <HomeSectionCarousel data={men_kurta} sectionName={"Women's Saree"}  />
                <HomeSectionCarousel data={men_kurta} sectionName={"Women's Dress"}  />
            </div>
        </div>
    );
}

export default HomePage;